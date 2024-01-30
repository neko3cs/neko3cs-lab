#include <QApplication>
#include <QPushButton>

int main(int argc, char** argv)
{
	QApplication app(argc, argv);
	QPushButton* button = new QPushButton("Hello Qt!");
	QPushButton* button2 = new QPushButton("Goodbye");
	button->show();
	button2->show();
	return app.exec();
}