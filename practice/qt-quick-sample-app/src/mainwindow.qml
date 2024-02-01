import QtQuick 2.13
import QtQuick.Controls 2.5
import QtQuick.Window 2.13
import QtQml.Models 2.13
import Element 1.0

ApplicationWindow {
  visible: true
  title: qsTr("Hello World")
  width: 640
  height: 480

  menuBar: MenuBar {
    Menu {
      title: qsTr("File")
      MenuItem {
        text: qsTr("&Open")
        onTriggered: console.log("Open action triggered");
      }
      MenuItem {
        text: qsTr("Exit")
        onTriggered: Qt.quit();
      }
    }
  }

  Button {
    text: qsTr("Hello World")
    anchors.horizontalCenter: parent.horizontalCenter
    anchors.verticalCenter: parent.verticalCenter
  }
}
