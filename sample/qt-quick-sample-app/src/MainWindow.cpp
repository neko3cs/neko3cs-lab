#include <QDebug>
#include "MainWindow.hpp"

MainWindow::MainWindow()
    : QObject(),
      m_labelText("Hello Qt Quick World!")
{
}

void MainWindow::updateLabel(const QString text)
{
  m_labelText = text;
  emit textChanged();
}

QString MainWindow::text() const
{
  return m_labelText;
}

void MainWindow::setText(const QString &text)
{
  if (text != m_labelText)
  {
    m_labelText = text;
    emit textChanged();
  }
}
