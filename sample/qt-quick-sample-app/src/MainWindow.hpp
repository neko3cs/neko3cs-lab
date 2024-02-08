#pragma once
#include <QObject>
#include <QString>

class MainWindow : public QObject
{
  Q_OBJECT
  Q_PROPERTY(QString labelText READ labelText WRITE setLabelText NOTIFY labelTextChanged)
  Q_PROPERTY(QString textFieldText READ textFieldText WRITE setTextFieldText NOTIFY textFieldTextChanged)

public:
  MainWindow();

  Q_INVOKABLE void updateLabel();

  QString labelText() const;
  void setLabelText(const QString &text);
  QString textFieldText() const;
  void setTextFieldText(const QString &text);

signals:
  void labelTextChanged();
  void textFieldTextChanged();

private:
  QString m_labelText;
  QString m_textFieldText;
};
