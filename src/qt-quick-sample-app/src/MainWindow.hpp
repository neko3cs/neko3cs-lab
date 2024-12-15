#pragma once
#include <QObject>
#include <QString>

class MainWindow : public QObject
{
  Q_OBJECT
  Q_PROPERTY(QString text READ text WRITE setText NOTIFY textChanged)

public:
  MainWindow();

  Q_INVOKABLE void updateLabel(const QString text);

  QString text() const;
  void setText(const QString &text);

signals:
  void textChanged();

private:
  QString m_labelText;
};
