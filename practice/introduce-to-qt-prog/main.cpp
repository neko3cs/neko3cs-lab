#include <QApplication>
#include <QLabel>
#include <QHBoxLayout>
#include <QLineEdit>

int main(int argc, char** argv)
{
	QApplication app(argc, argv);
	QLabel* label = new QLabel("Hello");
	QLineEdit* edit = new QLineEdit;
	QWidget* window = new QWidget;
	QHBoxLayout* layout = new QHBoxLayout;
	
	QObject::connect(edit,SIGNAL(textChanged(QString)),
			label,SLOT(setText(QString)) );
	QObject::connect(edit,SIGNAL(textChanged(QString)),
			window,SLOT(setWindowTitle(QString)) );
	
	layout->addWidget(edit);
	layout->addWidget(label);
	window->setLayout(layout);
	window->show();

	return app.exec();
}