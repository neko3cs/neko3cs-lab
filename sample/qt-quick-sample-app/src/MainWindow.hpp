#pragma once
#include <QQmlApplicationEngine>
#include <QObject>
#include <QString>

class MainWindow : public QObject
{
  Q_OBJECT
  Q_PROPERTY(QString labelText READ labelText WRITE setLabelText NOTIFY labelTextChanged)
  Q_PROPERTY(QString textFieldText READ textFieldText WRITE setTextFieldText NOTIFY textFieldTextChanged)

public:
  MainWindow(QQmlApplicationEngine &engine);

  Q_INVOKABLE void updateLabel();

  QString labelText() const;
  void setLabelText(const QString &text);
  QString textFieldText() const;
  void setTextFieldText(const QString &text);

signals:
  void labelTextChanged();
  void textFieldTextChanged();

private:
  QQmlApplicationEngine &m_engine;
  QString m_labelText;
  QString m_textFieldText;
};
