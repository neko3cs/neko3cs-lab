#include <QApplication>
#include "MainDialog.h"
#include "SecondDialog.h"

int main(int argc, char **argv)
{
  QApplication app(argc, argv);
  MainDialog *dialog = new MainDialog;
  dialog->show();
  return app.exec();
}