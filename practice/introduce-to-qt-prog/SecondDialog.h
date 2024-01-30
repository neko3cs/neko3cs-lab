#ifndef SECONDDIALOG_H_
#define SECONDDIALOG_H_

#include <QDialog>

class QPushButton;
class QLineEdit;
class QString;

class SecondDialog : public QDialog
{
  Q_OBJECT
public:
  SecondDialog(QWidget *parent = 0);
  QString getLineEditText();
signals:
  void okButtonClicked();

private:
  QPushButton *okButton;
  QPushButton *cancelButton;
  QLineEdit *editor;
};

#endif