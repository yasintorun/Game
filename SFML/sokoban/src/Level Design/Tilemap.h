#include <SFML/Graphics.hpp>
#include <list>
//#include "FileSystem.h"
enum class Mode
{
	None = -1,
	Wall,
	Box,
	Target,
	Player,
	Erase
};

class Tilemap /*: FileSystem*/ {
public:
	Tilemap();

	void Update();

	void Draw(sf::Vector2i);
	void Erase(sf::Vector2i);
	
	void HandleEvent(sf::Event, const sf::RenderWindow*);

	virtual void Display(sf::RenderWindow*);

	void save();

	virtual void load(std::string);

	void SetTexture(std::string);

	Mode mode;
private:
	bool drawing;
	sf::Texture text;
	sf::Sprite wallSprite, boxSprite, targetSprite, playerSprite, cursorSprite;
	
	//sf::VertexArray tile;
	//std::list<TileBase> tiles; //eger tek sprite yok ise bu durumu kullan. tilebase tilemapdaki 1 unit deki hem sprite'ý hem pozisyonu tutar. ek bir deger lazým ise tilebase de eklenir.
	std::list<sf::Vector2i> tile[3];// walls, boxes, targets;
	sf::Vector2i player;
};
