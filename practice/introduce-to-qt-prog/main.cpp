#include <QApplication>
#include <QPushButton>
#include <QGridLayout>

int main(int argc, char** argv)
{
	QApplication app(argc, argv);
	QWidget* window = new QWidget;
	QPushButton* buttonA = new QPushButton("Button A");
	QPushButton* buttonB = new QPushButton("Button B");
	QPushButton* buttonC = new QPushButton("Button C");
	QGridLayout* layout = new QGridLayout;

	layout->addWidget(buttonA,0,0);
	layout->addWidget(buttonB,0,1);
	layout->addWidget(buttonC,1,0,1,2);

	window->setLayout(layout);
	window->show();	
	return app.exec();
}