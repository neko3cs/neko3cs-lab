#ifndef MAINDIALOG_H_
#define MAINDIALOG_H_

#include <QDialog>

class QPushButton;
class QLabel;
class SecondDialog;

class MainDialog : public QDialog
{
  Q_OBJECT
public:
  MainDialog(QWidget *parent = 0);
public slots:
  void showSecondDialog();
  void setTextLabel();

private:
  QPushButton *showDialogButton;
  QLabel *textLabel;
  SecondDialog *secondDialog;
};

#endif