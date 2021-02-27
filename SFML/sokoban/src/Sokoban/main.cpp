#include "Game.h"

int main() {
	setlocale(LC_ALL, "tr");
	try
	{
		std::cout << "\
				\n[B�LG�] \n\
				\nHARAKET EDEB�LMEK ���N: OK TU�LARI\n\
				\nLEVEL� SIFIRLAMAK ���N: R\n\
				\nSONRAK� LEVEL ���N: P \n\
				\n�NCEK� LEVEL ���N: O\n\
				\n[B�LG�] \n\
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