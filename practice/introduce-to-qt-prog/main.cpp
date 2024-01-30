#include <QApplication>
#include <QPushButton>
#include <QHBoxLayout>
#include <QVBoxLayout>

int main(int argc, char** argv)
{
	QApplication app(argc, argv);
	QWidget* window = new QWidget;
	QPushButton* buttonA = new QPushButton("Button A");
	QPushButton* buttonB = new QPushButton("Button B");
	QPushButton* buttonC = new QPushButton("Button C");
	QPushButton* buttonD = new QPushButton("Button D");
	
	QVBoxLayout* mainLayout = new QVBoxLayout;
	QHBoxLayout* layoutA = new QHBoxLayout;
	QVBoxLayout* layoutB = new QVBoxLayout;

	layoutA->addWidget(buttonA);
	layoutA->addWidget(buttonB);
	layoutB->addWidget(buttonC);
	layoutB->addWidget(buttonD);

	mainLayout->addLayout(layoutA);
	mainLayout->addLayout(layoutB);

	window->setLayout(mainLayout);
	window->show();	
	return app.exec();
}