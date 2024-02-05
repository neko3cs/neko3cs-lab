#include <QGuiApplication>
#include <QQmlApplicationEngine>
#include <QQuickStyle>
#include "MainWindow.hpp"

int main(int argc, char *argv[])
{
  QGuiApplication app(argc, argv);

#if defined(Q_OS_MAC)
  QQuickStyle::setStyle("Material");
#endif

  QQmlApplicationEngine engine;
  MainWindow mainWindow(engine);
  QObject::connect(
      &engine,
      &QQmlApplicationEngine::objectCreationFailed,
      &app,
      []()
      { QCoreApplication::exit(-1); },
      Qt::QueuedConnection);
  engine.load(QUrl(u"qrc:/MainWindow.qml"_qs));

  return app.exec();
}
