#include <QtWidgets/QApplication>
#include <QPushButton>

int main(int argc, char **argv)
{
  QApplication app(argc, argv);
  QPushButton *button = new QPushButton("Hello Qt!");
  button->resize(200, 50);
  button->move(100, 50);
  button->show();
  return app.exec();
}