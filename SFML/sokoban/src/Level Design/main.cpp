#include "Tilemap.h"
#include <iostream>
using namespace sf;

//degiþkenler. ilk 3 ünü degiþtirebilirsin. gridx ve gridy ile oynama yapma.
constexpr int GameWidth = 1200;
constexpr int GameHeight = 800;
constexpr int size = 40;//	pixel per unit
constexpr int gridx = GameWidth / size;
constexpr int gridy = GameHeight / size;



struct Grid {
	Vertex point1, point2;
	Grid(){}
	Grid(Vector2f x, Vector2f y) {
		point1 = x;
		point2 = y;
	}
	void Draw(RenderWindow* w) {
		Vertex line[2] = { point1,point2 };
		line[0].color.a = line[1].color.a = 50;
		w->draw(line, 2, Lines);
	}
};


int main() {

	setlocale(0, "tr");
	std::cout<<
				"\
				\n[BÝLGÝ] \n\
				\nÇÝZÝM YAPMAK ÝÇÝN: D\n\
				\nSÝLMEK ÝÇÝN: E\n\
				\nBOÞ DURUM ÝÇÝN: N \n\
				\nDUVAR ÝÇÝN: W\n\
				\nKUTU ÝÇÝN: B\n\
				\nOYUNCU ÝÇÝN: P\n\
				\nHEDEF NOKTASI ÝÇÝN: T\n\
				\nTuþuna basýnýz.\n\
				\n[BÝLGÝ] \n\
				\n";


	RenderWindow window(VideoMode(GameWidth, GameHeight), "Sokoban Level Design", Style::Close);
	//Ekrandaki Beyaz çizgiler.
	Grid grid[gridx + gridy];
	for(int x = 0; x<gridx; x++)
		grid[x] = Grid(Vector2f(x * size, 0), Vector2f(x*size,GameHeight));

	for (int y = 0; y < gridy; y++)
		grid[y+gridx] = Grid(Vector2f(0, y * size), Vector2f(GameWidth, y * size));

	Tilemap tilemap = Tilemap();
	//tilemap.load("level2");
	bool isSave = false;
	while (window.isOpen()) {

		Event e;
		while (window.pollEvent(e)) {


			if (e.type == Event::Closed)
				window.close();
			

			tilemap.HandleEvent(e, &window);
		}

		window.clear();


		for (int i = 0; i < gridx+gridy; i++)
			grid[i].Draw(&window);

		tilemap.Display(&window);
		window.display();

	}


	return 0;
}