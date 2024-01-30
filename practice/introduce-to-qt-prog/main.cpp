#include <QApplication>
#include <QPushButton>
#include <QHBoxLayout>

int main(int argc, char** argv)
{
	QApplication app(argc, argv);
	QWidget* window = new QWidget;
	QPushButton* buttonA = new QPushButton("Button A");
	QPushButton* buttonB = new QPushButton("Button B");
	QPushButton* buttonC = new QPushButton("Button C");
	QHBoxLayout* layout = new QHBoxLayout;

	layout->addWidget(buttonA);
	layout->addWidget(buttonB);
	layout->addWidget(buttonC);

	window->setLayout(layout);
	window->show();	
	return app.exec();
}