#include "Tilemap.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
Tilemap::Tilemap() {
	drawing = false;
	mode = Mode::None;
	player = sf::Vector2i(-1, -1);
	SetTexture("resources/sokoban.png"); //resim yolunu belirt.
}
void Tilemap::SetTexture(std::string filename) {
	if (!text.loadFromFile(filename)) {
		sf::Image img;
		img.create(40, 40, sf::Color::White);
		img.saveToFile(filename);
		text.loadFromFile(filename);
	}
	//resmi parçala. resim -> 160x40 boyutunda olmalý.
	wallSprite = sf::Sprite(text, sf::IntRect(0, 0, 40, 40));
	boxSprite = sf::Sprite(text, sf::IntRect(120, 0, 40, 40));
	targetSprite = sf::Sprite(text, sf::IntRect(80, 0, 40, 40));
	playerSprite = sf::Sprite(text, sf::IntRect(40, 0, 40, 40));
}

void Tilemap::Update() {

	
}

void Tilemap::Draw(sf::Vector2i pos) {

	if(mode == Mode::Player && player.x == -1) {
		player = pos;
		return;
	}
	int m = (int)mode;
	std::list<sf::Vector2i>* l;
	//Buradaki kontrole aslýnda gerek yok. ama biz yinede dizinin taþma ihtimaline karþý önlemimizi alalým.
	if (m > -1 && m < 3)
		l = &tile[m];
	else
		return;
	
	auto it = std::find(l->begin(), l->end(), pos);
	if(it == l->end())
		l->push_back(pos);
	
}

void Tilemap::Erase(sf::Vector2i pos) {
	for(int i = 0; i<3; i++) {
		tile[i].remove(pos);
	}
	if (player == pos)
		player = sf::Vector2i(-1, -1);
}

void Tilemap::HandleEvent(sf::Event e, const sf::RenderWindow* w) {
	
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::LControl))
		if (sf::Keyboard::isKeyPressed(sf::Keyboard::S))
			save();

	if(e.type == sf::Event::KeyPressed) {
		switch (e.key.code)
		{
		case sf::Keyboard::W:
			mode = Mode::Wall;
			break;
		case sf::Keyboard::B:
			mode = Mode::Box;
			break;
		case sf::Keyboard::T:
			mode = Mode::Target;
			break;
		case sf::Keyboard::P:
			mode = Mode::Player;
			break;
		case sf::Keyboard::E:
			mode = Mode::Erase;
			break;
		case sf::Keyboard::N:
			mode = Mode::None;
			break;

		//C tuþuna basýldýgýnda list temizle.
		case sf::Keyboard::C:
			for (int i = 0; i < 3; i++)
				tile[i].clear();
			player.x = player.y = -1;
			break;
		}
	}

	sf::Vector2i mousePos = sf::Mouse::getPosition(*w);

	
	if (e.type == sf::Event::MouseButtonPressed)	drawing = true;
	if (e.type == sf::Event::MouseButtonReleased)	drawing = false;
	if (drawing && mode != Mode::None) {
		if (mode == Mode::Erase)
			Erase(mousePos / 40);
		else
			Draw(mousePos / 40);
	}

} 


void Tilemap::Display(sf::RenderWindow* w) {
	for (int i = 0; i < 3; i++) {
		sf::Sprite *sprite;
		if (i == 0)
			sprite = &wallSprite;
		else
			sprite = i == 2 ? &boxSprite : &targetSprite;
		for (auto it = tile[i].begin(); it != tile[i].end(); it++) {
			sf::Vector2f v2 = sf::Vector2f(*it * 40);
			sprite->setPosition(v2);
			w->draw(*sprite);
		}
	}

	if (player.x != -1) {
		playerSprite.setPosition(sf::Vector2f(player * 40));
		w->draw(playerSprite);
	}
	//w->draw(cursorSprite);
}

void Tilemap::save() {
	std::string filename;
	std::cout << "Level ismi: ";
	std::cin >> filename;
	std::string title[] = { "#Duvar", "#Kutu", "#Hedef", "#Oyuncu" };
	std::ofstream file;
	file.open("level"+filename+".lvl", std::ios::out);
	for(int i = 0; i<3; i++) {
		file << title[i] << std::endl;
		for(auto it = tile[i].begin(); it != tile[i].end(); it++) {
			file << (*it).x << " " << (*it).y << std::endl;
		}
	}
	file << title[3]<<std::endl;
	file << player.x << " " << player.y << std::endl<<'#';
}

void Tilemap::load(std::string lvl_name) {
	std::string str;
	std::ifstream file;
	file.open(lvl_name+".lvl", std::ios::in);
	int index = -1;
	while(index< 4) {
		std::getline(file, str);
		if (str[0] == '#') {
			index++;
			continue;
		}
		else {
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
}

