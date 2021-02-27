#include <iostream>
#include <list>
#include <fstream>
#include <sstream>
#include <SFML/Graphics.hpp>

class Level
{
public:

	Level();

	bool Load();

	void Clear();

	void Display(sf::RenderWindow* w);

	sf::Vector2i getPlayer();
	void setPlayer(sf::Vector2i npos);

	bool collision(sf::Vector2i collision, bool isPlayer = true);

	bool Completed();
	void changeLevel(int);

private:
	sf::Texture text;
	sf::Sprite wallSprite, boxSprite, targetSprite, playerSprite, cursorSprite;

	std::list<sf::Vector2i> tile[3];// walls, boxes, targets;
	sf::Vector2i player;

	int currentLevel =1;
};

