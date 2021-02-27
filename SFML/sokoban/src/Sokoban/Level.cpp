#include "Level.h"
#include "config.h"

Level::Level()
{
	currentLevel = 1;
	if (!text.loadFromFile(path+"sokoban.png"))
	{
		//resim yoksa boþ bir resim oluþtur.
		sf::Image img;
		img.create(40, 40, sf::Color::White);
		img.saveToFile(path + "sokoban.png");
		text.loadFromFile(path + "sokoban.png");
	}
	//resmi parçala. resim -> 160x40 boyutunda olmalý.
	wallSprite = sf::Sprite(text, sf::IntRect(0, 0, 40, 40));
	boxSprite = sf::Sprite(text, sf::IntRect(120, 0, 40, 40));
	targetSprite = sf::Sprite(text, sf::IntRect(80, 0, 40, 40));
	playerSprite = sf::Sprite(text, sf::IntRect(40, 0, 40, 40));
}

void Level::Clear()
{
	for (int i = 0; i < 3; i++)
		tile[i].clear();
	player = sf::Vector2i(0, 0);
}

bool Level::Load()
{
	Clear();
	std::string str;
	std::ifstream file;
	std::string lvl_name = "level" + std::to_string(currentLevel) + ".lvl";
	file.open(path+lvl_name, std::ios::in);
	if (!file.is_open()) return false;
	int index = -1;
	while (index < 4)
	{
		std::getline(file, str);
		if (str[0] == '#' || file.eof())
		{
			index++;
			continue;
		}
		else
		{
			int x = -1, y = -1;
			std::stringstream s(str);
			s >> x >> y;
			sf::Vector2i v2i = sf::Vector2i(x, y);
			if (index < 3)
				tile[index].push_back(v2i);
			else
				player = v2i;
		}
	}
	return true;
}


void Level::Display(sf::RenderWindow* w)
{
	for (int i = 0; i < 3; i++)
	{
		sf::Sprite* sprite;
		if (i == 0)
			sprite = &wallSprite;
		else
			sprite = i == 2 ? &boxSprite : &targetSprite;
		for (auto it = tile[i].begin(); it != tile[i].end(); it++)
		{
			sf::Vector2f v2 = sf::Vector2f(*it * 40);
			sprite->setPosition(v2);
			w->draw(*sprite);
		}
	}

	if (player.x != -1)
	{
		playerSprite.setPosition(sf::Vector2f(player * 40));
		w->draw(playerSprite);
	}
}

//get set

sf::Vector2i Level::getPlayer()
{
	return player;
}

void Level::setPlayer(sf::Vector2i npos)
{
	player = npos;
}

bool Level::collision(sf::Vector2i collision, bool isPlayer)
{

	//eger yeni konumda duvar var ise o konuma hiçbir þekilde gidilemez.
	for (auto wall : tile[0])
		if (wall == collision)
			return false;


	//kutu çarpýþmalarýný denetle.
	for (auto it = tile[1].begin(); it != tile[1].end(); it++)
	{
		
		if (*it == collision)
		{
			//eðer fonksiyon player için çagrýlmýþsa kutu hareket etmeli. Ama kutunun yeni yerinde baþka bir kutu veya duvar olabilir. bu durumu kontrol etmeliyiz.
			if (isPlayer)
			{
				sf::Vector2i newBoxPos = *it;
				int x = it->x - player.x;
				int y = it->y - player.y;
				newBoxPos += sf::Vector2i(x, y); //box'ýn yeni konumu.
				
                //Yeni kutunun çarpýþmalarýný kontrol et.
				if (this->collision(newBoxPos, false))
				{
					//eðer yeni yer boþ ise o yere taþý.
					*it = newBoxPos;

					//Görev tamam!
					if (Completed())
					{
						++currentLevel;
						Load();
					}
					return true;
				}
			}
			
			return false;
			
		}
	}
	return true;
}


// bu fonksiyonu iyileþtir.
bool Level::Completed()
{
	int counter = 0;
	for (auto box : tile[1])
		for (auto target : tile[2])
			if (box == target)
			{
				counter++;
				break;
			}
	return(counter == tile[2].size());
}


void Level::changeLevel(int value)
{
	if (value == 0 ||(value == -1 && currentLevel == 0) ||(value == 1 && currentLevel == MAX_LEVEL))return;
	value = value < 0 ? -1 : 1;
	currentLevel += value;
	if (!Load())
		std::cout << "asd\n";
}



