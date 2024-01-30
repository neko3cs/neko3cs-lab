#ifndef MAIN_H_
#define MAIN_H_

#include <QDialog>

class QLabel;
class QPushButton;
class QLineEdit;

class MainDialog : public QDialog
{
	Q_OBJECT
public:
	MainDialog(QWidget *parent = 0);
private slots:
	void setLabelText();

private:
	QLabel *label;
	QPushButton *setButton;
	QLineEdit *lineEdit;
};

#endif