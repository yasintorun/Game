#include "Game.h"

int main() {
	setlocale(LC_ALL, "tr");
	try
	{
		std::cout << "\
				\n[BÝLGÝ] \n\
				\nHARAKET EDEBÝLMEK ÝÇÝN: OK TUÞLARI\n\
				\nLEVELÝ SIFIRLAMAK ÝÇÝN: R\n\
				\nSONRAKÝ LEVEL ÝÇÝN: P \n\
				\nÖNCEKÝ LEVEL ÝÇÝN: O\n\
				\n[BÝLGÝ] \n\
				\n";

		Game game;
		game.Run();
		std::cout << "\n\nOyunumu Oynadigin icin tesekurler.\n\n";
	}
	catch (const std::exception& e)
	{
		std::cout << e.what() << std::endl;
	}


	return 0;
}