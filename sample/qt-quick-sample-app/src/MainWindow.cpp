#include <QQmlApplicationEngine>
#include <QQmlContext>
#include <QDebug>
#include "MainWindow.hpp"

MainWindow::MainWindow(QQmlApplicationEngine &engine)
    : QObject(), m_engine(engine), m_text("Hello Qt Quick World!")
{
  m_engine.rootContext()->setContextProperty("mainWindow", this);
}

void MainWindow::updateLabel(const QString &text)
{
  m_text = text;
  emit textChanged();
}

QString MainWindow::text() const
{
  return m_text;
}

void MainWindow::setText(const QString &text)
{
  if (text != m_text)
  {
    m_text = text;
    emit textChanged();
  }
}
