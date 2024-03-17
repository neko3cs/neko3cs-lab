#include <QGuiApplication>
#include <QQmlApplicationEngine>
#include <QQmlContext>
#include <QQuickStyle>
#include "MainWindow.hpp"

int main(int argc, char *argv[])
{
  QGuiApplication app(argc, argv);

#if defined(Q_OS_MAC)
  QQuickStyle::setStyle("Material");
#endif

  MainWindow mainWindow; // engineの前にmainWindowを宣言しないとpropertyがnullだという実行時エラーが出る
  QQmlApplicationEngine engine;
  engine.rootContext()->setContextProperty("mainWindow", &mainWindow);
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
