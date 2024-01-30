#include <QPushButton>
#include <QLabel>
#include <QLineEdit>
#include <QHBoxLayout>
#include "SecondDialog.h"

SecondDialog::SecondDialog(QWidget *parent) : QDialog(parent)
{
  okButton = new QPushButton(tr("&OK"));
  cancelButton = new QPushButton(tr("&Cancel"));
  editor = new QLineEdit;

  QHBoxLayout *layout = new QHBoxLayout;
  layout->addWidget(editor);
  layout->addWidget(okButton);
  layout->addWidget(cancelButton);
  setLayout(layout);

  connect(okButton, SIGNAL(clicked()), this, SLOT(accept()));
  connect(cancelButton, SIGNAL(clicked()), this, SLOT(reject()));
}

QString SecondDialog::getLineEditText()
{
  return editor->text();
}