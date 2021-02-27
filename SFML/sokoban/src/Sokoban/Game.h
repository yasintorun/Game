#include "Level.h"
class Game
{
public:
	Game();

	void Run();

	void HandleEvent();


	void Draw();

private:

	//SFML
	sf::RenderWindow* window;
	sf::Event* event;

	//Level
	Level level;

	//Data
	bool isLoading = false;
};

