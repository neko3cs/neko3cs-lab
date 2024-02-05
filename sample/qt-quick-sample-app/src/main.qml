import QtQuick 2.15
import QtQuick.Controls 2.15

ApplicationWindow {
    visible: true
    width: 640
    height: 480
    title: "Hello World App"

    Item {
        anchors.fill: parent

        Rectangle {
            width: parent.width
            height: parent.height
            color: "white"

            Text {
                text: "Hello, World!"
                font.pixelSize: 24
                color: "black"
                anchors.centerIn: parent
            }
        }
    }
}
