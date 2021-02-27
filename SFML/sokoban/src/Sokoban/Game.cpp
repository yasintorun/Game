#include "Game.h"
#include "config.h"
Game::Game()
{
	window = new sf::RenderWindow(sf::VideoMode(GAME_WIDTH, GAME_HEIGHT), GAME_TITLE, sf::Style::Close);
	event = new sf::Event;
	isLoading = level.Load();

	if (!isLoading)
		std::cout << "Level Yüklenemedi\n";

}

void Game::Run()
{
	while (window->isOpen()  && isLoading)
	{
		while (window->pollEvent(*event))
		{
			if (event->type == sf::Event::Closed)
				window->close();


			
			HandleEvent();
		}

		window->clear();

		Draw();

		window->display();

	}


}


//olaylarý yakala.
void Game::HandleEvent()
{
	if (event->type == sf::Event::KeyPressed)
	{
		//Oyuncu Kontrolü
		sf::Vector2i player = level.getPlayer();
		if (event->key.code == sf::Keyboard::Left)
			player.x -= 1;
		if (event->key.code == sf::Keyboard::Right)
			player.x += 1;
		if (event->key.code == sf::Keyboard::Up)
			player.y -= 1;
		if (event->key.code == sf::Keyboard::Down)
			player.y += 1;
		if (level.collision(player))
		{
			level.setPlayer(player);
		}

		if (event->key.code == sf::Keyboard::R)
			level.Load();

		if (event->key.code == sf::Keyboard::P)
			level.changeLevel(1);
		if (event->key.code == sf::Keyboard::O)
			level.changeLevel(-1);

	}
}


//ekrana çiz.
void Game::Draw()
{
	level.Display(window);
}

