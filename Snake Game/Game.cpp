#include "Game.h"

/////////////
Game::Game() 
{
	srand(time(NULL));
	window = new RenderWindow(VideoMode(GAMEWIDTH, GAMEHEIGHT), GAMETITLE);
	if (!gameTexture.loadFromFile("images.png"))
	{
		//eger belirtilen yolda oyun resimleri mevcut degilse sifirdan bir resim yaratiyoruz.
		sf::Image image;
		image.create(160, 40, Color::White);

		//snake: blue, tile:green, apple:red, edge:black
		Color c[4] = { Color::Blue, Color::Green, Color::Red, Color::Black };

		for (int x = 0; x < 4; x++)
			for (int i = 0; i < SNAKESIZE; i++)
				for (int j = 0; j < SNAKESIZE; j++)
					image.setPixel(i + SNAKESIZE * x, j, c[x]);

		if (!image.saveToFile("images.png"))window->close();//eger image olusmazsa sorun var demektir. oyunu kapat.
		gameTexture.loadFromFile("images.png");
	}

	//resmi parcaliyoruz.
	snakeBody = Sprite(gameTexture, IntRect(0, 0, 40, 40));
	tile = Sprite(gameTexture, IntRect(40, 0, 40, 40));
	appleSprite = Sprite(gameTexture, IntRect(80, 0, 40, 40));
	edge = Sprite(gameTexture, IntRect(120, 0, 40, 40));

	for (int i = 1; i < MAXSNAKELENGTH; i++)
	{
		snake[i].x = -1;
	}
	snake[0] = Vector2i(gridx/2, gridy/2); //pencerenin ortasina aliyoruz.
	do
	{
		apple = Vector2i(rand() % gridx, rand() % gridy);
	} while (search(snake, apple, gameMode));
	

	appleSprite.setPosition(apple.x * SNAKESIZE, apple.y * SNAKESIZE);

}

/////////////
void Game::MenuScene() 
{
	Font titleFont, textFont;
	titleFont.loadFromFile("title.otf");
	textFont.loadFromFile("text.otf");

	Text gameTitleText(MenuGameTitle, titleFont, MenuGameTitleCharSize);
	gameTitleText.setPosition((GAMEWIDTH - gameTitleText.getGlobalBounds().width) / 2, GAMEHEIGHT / 2 - 300);
	gameTitleText.setFillColor(buttonNormal);

	Text newGameText(newGameTxt, textFont, MenuNormalTextSize);
	newGameText.setPosition((GAMEWIDTH - newGameText.getGlobalBounds().width) / 2, GAMEHEIGHT / 2 - 100);
	newGameText.setFillColor(buttonNormal);

	Text exitGameText(ExitGameTxt, textFont, MenuNormalTextSize);
	exitGameText.setPosition((GAMEWIDTH - exitGameText.getGlobalBounds().width) / 2, GAMEHEIGHT / 2 );
	exitGameText.setFillColor(buttonNormal);


	while(window->isOpen())
	{
		Event* e = new Event();
		while(window->pollEvent(*e))
		{
			if (e->type == Event::Closed)
				window->close();

			Vector2f getPos = static_cast<Vector2f>(Mouse::getPosition(*window));

			if (e->type == Event::MouseButtonPressed)
			{
				if (newGameText.getGlobalBounds().contains(getPos))
					newGameText.setFillColor(ButtonPressed);
				else if (exitGameText.getGlobalBounds().contains(getPos))
					exitGameText.setFillColor(ButtonPressed);
			}
			else if (e->type == Event::MouseButtonReleased)
			{
				if (newGameText.getGlobalBounds().contains(getPos)) 
				{
					newGameText.setFillColor(buttonNormal);
					LevelScene();

				}
				else if (exitGameText.getGlobalBounds().contains(getPos))
				{
					exitGameText.setFillColor(buttonNormal);
					window->close();
					break;
				}
			}
			else if (e->type == Event::MouseMoved)
			{
				BUTTON_CHANGE_COLOR(newGameText, buttonHighlighted);
				BUTTON_CHANGE_COLOR(exitGameText, buttonHighlighted);
			}

		}

		window->clear(GameBackgroundColor);
		window->draw(gameTitleText);
		window->draw(newGameText);
		window->draw(exitGameText);
		window->display();
	}
}

/////////////
void Game::LevelScene() 
{
	Font textFont;
	textFont.loadFromFile("text.otf");

	Text ClassicGameText(classicGameModeTxt, textFont, MenuNormalTextSize);
	ClassicGameText.setPosition((GAMEWIDTH - ClassicGameText.getGlobalBounds().width) / 2, GAMEHEIGHT / 2 - 150);
	ClassicGameText.setFillColor(buttonNormal);
	Text EdgeGameText(edgeGameModeTxt, textFont, MenuNormalTextSize);
	EdgeGameText.setPosition((GAMEWIDTH - EdgeGameText.getGlobalBounds().width) / 2, GAMEHEIGHT / 2 - 50);
	EdgeGameText.setFillColor(buttonNormal);
	while(window->isOpen())
	{
		Event* e = new Event();
		while(window->pollEvent(*e))
		{
			if (e->type == Event::Closed)
				window->close();

			Vector2f getPos = static_cast<Vector2f>(Mouse::getPosition(*window));

			if (e->type == Event::MouseButtonPressed)
			{
				if (ClassicGameText.getGlobalBounds().contains(getPos))
					ClassicGameText.setFillColor(ButtonPressed);
				else if (EdgeGameText.getGlobalBounds().contains(getPos))
					EdgeGameText.setFillColor(ButtonPressed);
			}
			else if (e->type == Event::MouseButtonReleased)
			{
				if (ClassicGameText.getGlobalBounds().contains(getPos))
				{
					ClassicGameText.setFillColor(buttonNormal);
					gameMode = GameMode::ClassicGameMode;
					Run();
				}
				else if (EdgeGameText.getGlobalBounds().contains(getPos))
				{
					EdgeGameText.setFillColor(buttonNormal);
					gameMode = GameMode::EdgeGameMode;
					Run();
				}
			}
			else if (e->type == Event::MouseMoved)
			{
				BUTTON_CHANGE_COLOR(ClassicGameText, buttonHighlighted);
				BUTTON_CHANGE_COLOR(EdgeGameText, buttonHighlighted);
			}

		}

		window->clear(GameBackgroundColor);
		window->draw(ClassicGameText);
		window->draw(EdgeGameText);
		window->display();

	}
}


/////////////
void Game::Run()
{
	while (window->isOpen()) {
		GameUpdate();
	}
}

/////////////
void Game::GameUpdate() 
{
	timer += clock.getElapsedTime().asSeconds();
	clock.restart();

	Event* e = new Event();

	HandleEvent(*e);

	if (timer > delay && !gamePaused)
	{
		for (int i = snakeIndex; i > 0; i--)
			snake[i] = snake[i - 1];

		SnakeMove();

		if (search(snake, snake[0], gameMode))
			Reset();

		if (snake[0] == apple)
			EatApple();

		timer = 0;
	}
	GameDraw();
}

/////////////
void Game::HandleEvent(Event e)
{
	while (window->pollEvent(e))
	{
		if (e.type == Event::Closed)
			window->close();
		if (e.type == Event::KeyPressed)
		{
			if (e.key.code == Keyboard::A && dir != 2 && !gamePaused) dir = 1;
			else if (e.key.code == Keyboard::D && dir != 1 && !gamePaused) dir = 2;
			else if (e.key.code == Keyboard::W && dir != 4 && !gamePaused) dir = 3;
			else if (e.key.code == Keyboard::S && dir != 3 && !gamePaused) dir = 4;
			else if (e.key.code == Keyboard::Escape) gamePaused = !gamePaused;
		}
	}
}

/////////////
void Game::SnakeMove()
{
	if (dir == 1)	snake[0].x--;
	else if (dir == 2)	snake[0].x++;
	else if (dir == 3)	snake[0].y--;
	else if (dir == 4)	snake[0].y++;
}

/////////////
void Game::EatApple() 
{
	do
	{
		apple = Vector2i(rand() % gridx, rand() % gridy);
	} while (search(snake, apple, gameMode));

	appleSprite.setPosition(apple.x * SNAKESIZE, apple.y * SNAKESIZE);
	snakeIndex++;
	score++;
}

/////////////
void Game::Reset() 
{
	dir = 0;
	for (int i = 1; i < MAXSNAKELENGTH; i++)
		snake[i].x = -1;
	snakeIndex = -1;
	snake[0] = apple = Vector2i(8, 7);

	LevelScene();
}

/////////////
void Game::GameDraw() 
{
	window->clear(GameBackgroundColor);

	//draw tile
	for (int i = 0; i <= gridx; i++)
	{
		for (int j = 0; j <= gridy; j++)
		{
			tile.setPosition(i * SNAKESIZE, j * SNAKESIZE);
			window->draw(tile);
		}
	}

	//draw snake
	for (int i = 0; i <= snakeIndex; i++)
	{
		snakeBody.setPosition(snake[i].x * SNAKESIZE, snake[i].y * SNAKESIZE);
		window->draw(snakeBody);
	}

	//draw edge
	if (gameMode == GameMode::EdgeGameMode) {
		for (int i = 0; i <= gridx; i++)
		{
			for (int j = 0; j <= gridy;)
			{
				edge.setPosition(i * SNAKESIZE, j * SNAKESIZE);
				window->draw(edge);
				if (i != 0 && i != gridx)
					j += gridy;
				else
					j++;
			}
		}
	}
	//draw apple
	window->draw(appleSprite);
	window->display();
}
