#include <QtWidgets/QApplication>
#include <QPushButton>
#include <QFont>

int main(int argc, char **argv)
{
  QApplication app(argc, argv);
  QPushButton *button = new QPushButton("Hello Qt!");
  button->resize(200, 50);
  button->move(100, 50);
  button->setFont(QFont("Times", 15, QFont::Bold, true));
  button->show();
  return app.exec();
}