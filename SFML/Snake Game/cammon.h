#include <SFML/Graphics.hpp>
using namespace sf;

#define GAMEWIDTH 800
#define GAMEHEIGHT 600
#define GAMETITLE "Yilan Oyunu"
#define SNAKESIZE 40
#define MAXSNAKELENGTH 100
static int snakeIndex = 0;
static int score = 0;

const Color GameBackgroundColor = Color(41, 42, 71); //oyunun arka plan rngi
const Color buttonNormal = Color(76, 220, 180); //normal rengi.
const Color buttonHighlighted = Color(163, 238, 219); //ustune gelindiginde.
const Color ButtonPressed = Color(40, 95, 80); //tiklandiginda
#define BUTTON_CHANGE_COLOR(B, C) B.setFillColor(B.getGlobalBounds().contains(getPos) ? C : buttonNormal);


const int gridx = GAMEWIDTH / SNAKESIZE - 1;
const int gridy = GAMEHEIGHT / SNAKESIZE - 1;

const int MenuGameTitleCharSize = 120;
const int MenuNormalTextSize = 50;

const String MenuGameTitle = "(YILAN OYUNU)";
const String newGameTxt = "Yeni Oyun";
const String ExitGameTxt = "Oyundan Cik";
const String classicGameModeTxt = "Klasik Oyun";
const String edgeGameModeTxt = "Kenarli Oyun"; //bu modun ismi neydi ya :D?
const String scoreText = "Skor: ";

enum class GameMode
{
	ClassicGameMode,
	EdgeGameMode
};

inline bool search(const Vector2i* v1, Vector2i& v2, GameMode gm)
{
	//eger oyun modu klasik ise yilan kenarlardan gecebilsin.
	if(gm == GameMode::ClassicGameMode) {
		if (v2.x < 0) v2.x = gridx;
		else if (v2.x > gridx) v2.x = 0;
		else if (v2.y < 0) v2.y = gridy;
		else if (v2.y > gridy) v2.y = 0;
	}
	//Eger oyun modu kenarli ise yilan kenarlara carpýnca ölsün.
	else if(gm == GameMode::EdgeGameMode)
		if (v2.x < 1 || v2.x > gridx - 1 || v2.y<1 || v2.y>gridy - 1)
			return true;
	for (int i = 1; i <= snakeIndex; i++)
		if (v1[i] == v2)
			return true;
	return false;
}
