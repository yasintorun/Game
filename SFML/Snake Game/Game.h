#include "cammon.h"

class Game
{
private:
	RenderWindow* window;
	Texture gameTexture;
	bool gamePaused = false;
	Vector2i snake[MAXSNAKELENGTH];
	Vector2i apple;
	Sprite snakeBody, tile, appleSprite, edge;
	Clock clock;
	GameMode gameMode;
	int dir = 0;
	float timer = 0, delay = 0.12;

public:

	Game();

	//oyunun ana dongusu.
	void Run();

	void MenuScene();

	void LevelScene();

	//olaylar.
	void HandleEvent(Event);

	//oyun guncellemesi
	void GameUpdate();

	//yilanin hareketi
	void SnakeMove();

	//oyunu sifirla
	void Reset();

	//elmayi ye.
	void EatApple();

	//oyunu ekrana ciz
	void GameDraw();
};

