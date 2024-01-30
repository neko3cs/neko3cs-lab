#ifndef MAINDIALOG_H_
#define MAINDIALOG_H_

#include <QDialog>

class QPushButton;
class QLabel;

class MainDialog : public QDialog
{
  Q_OBJECT
public:
  MainDialog(QWidget *parent = 0);
private slots:
  void showSecondDialog();

private:
  QPushButton *showDialogButton;
  QLabel *textLabel;
};

#endif