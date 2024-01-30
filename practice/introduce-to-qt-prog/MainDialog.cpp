#include <QPushButton>
#include <QLabel>
#include <QHBoxLayout>
#include "MainDialog.h"
#include "SecondDialog.h"

MainDialog::MainDialog(QWidget *parent) : QDialog(parent)
{
  showDialogButton = new QPushButton("Show Second Dialog");
  textLabel = new QLabel("empty");
  connect(showDialogButton, SIGNAL(clicked()), this, SLOT(showSecondDialog()));

  QHBoxLayout *layout = new QHBoxLayout;
  layout->addWidget(textLabel);
  layout->addWidget(showDialogButton);
  setLayout(layout);
}

void MainDialog::showSecondDialog()
{
  SecondDialog secondDialog(this);
  if (secondDialog.exec())
  {
    QString str = secondDialog.getLineEditText();
    textLabel->setText(str);
  }
}