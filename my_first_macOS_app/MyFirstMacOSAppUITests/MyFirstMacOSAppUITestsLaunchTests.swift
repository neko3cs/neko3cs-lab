//
//  MyFirstMacOSAppUITestsLaunchTests.swift
//  MyFirstMacOSAppUITests
//
//  Created by neko3cs on 2026/02/14.
//

import XCTest

final class MyFirstMacOSAppUITestsLaunchTests: XCTestCase {

    override class var runsForEachTargetApplicationUIConfiguration: Bool {
        true
    }

    override func setUpWithError() throws {
        continueAfterFailure = false
    }

    @MainActor
    func testLaunch() throws {
        let app = XCUIApplication()
        app.launch()

        // アプリが起動してメイン画面が表示されるのを待機
        XCTAssertTrue(app.staticTexts["カウンター"].waitForExistence(timeout: 5))

        let attachment = XCTAttachment(screenshot: app.screenshot())
        attachment.name = "Launch Screen"
        attachment.lifetime = .keepAlways
        add(attachment)
    }
}
