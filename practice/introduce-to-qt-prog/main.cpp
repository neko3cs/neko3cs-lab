#include <QtGui>
#include <QApplication>
#include <QLabel>
#include <QPushButton>
#include <QLineEdit>
#include <QHBoxLayout>
#include "main.h"

MainDialog::MainDialog(QWidget *parent)
    : QDialog(parent)
{
  label = new QLabel(tr("empty"));
  setButton = new QPushButton(tr("Set"));
  lineEdit = new QLineEdit;

  connect(setButton, SIGNAL(clicked()), this, SLOT(setLabelText()));

  QHBoxLayout *layout = new QHBoxLayout;
  layout->addWidget(lineEdit);
  layout->addWidget(label);
  layout->addWidget(setButton);
  setLayout(layout);
}

void MainDialog::setLabelText()
{
  QString text = lineEdit->text();
  label->setText(text);
}

int main(int argc, char **argv)
{
  QApplication app(argc, argv);
  MainDialog *dialog = new MainDialog;
  dialog->show();
  return app.exec();
}