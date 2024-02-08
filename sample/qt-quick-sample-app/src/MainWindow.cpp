#include <QQmlApplicationEngine>
#include <QQmlContext>
#include <QDebug>
#include "MainWindow.hpp"

MainWindow::MainWindow(QQmlApplicationEngine &engine)
    : QObject(), m_engine(engine), m_labelText("Hello Qt Quick World!")
{
  m_engine.rootContext()->setContextProperty("mainWindow", this);
  m_engine.setInitialProperties({{"textFieldText", QVariant::fromValue(m_textFieldText)}});
}

void MainWindow::updateLabel(const QString text)
{
  qDebug() << "text: " << text; // DEBUG

  m_labelText = text;
  emit labelTextChanged();
}

QString MainWindow::labelText() const
{
  return m_labelText;
}

void MainWindow::setLabelText(const QString &text)
{
  if (text != m_labelText)
  {
    m_labelText = text;
    emit labelTextChanged();
  }
}

QString MainWindow::textFieldText() const
{
  return m_textFieldText;
}

void MainWindow::setTextFieldText(const QString &text)
{
  if (text != m_textFieldText)
  {
    m_textFieldText = text;
    emit textFieldTextChanged();
  }
}
