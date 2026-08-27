# Municipal Citizen Reporting

## Project overview

Municipal Citizen Reporting is a C# Windows Forms application that lets residents submit municipal service issues such as potholes, water leaks, electricity faults, sanitation concerns, and streetlight problems. The current academic-project scope implements issue reporting and leaves the other planned municipal services visible but unavailable.

Submitted reports receive an in-memory reference number beginning at `REQ-1001` and the status `Submitted`. Each report captures the reference number, location, municipal category, description, optional supporting file path, submission date and time, and current status. Residents can view all reports captured during the current application session on a read-only history screen.

## Requirements

- Windows 10 or Windows 11
- Visual Studio 2022 or a newer Visual Studio version that supports .NET Framework desktop development
- The **.NET desktop development** Visual Studio workload
- .NET Framework 4.8 Developer Pack/targeting pack

No database, web service, third-party package, or internet connection is required.

## How to compile

1. Open `MunicipalCitizenReporting.sln` in Visual Studio.
2. If Visual Studio prompts for missing components, install the .NET desktop development workload and the .NET Framework 4.8 targeting pack through Visual Studio Installer.
3. Select `Debug` and `Any CPU` from the solution configuration controls.
4. Choose **Build > Build Solution**, or press `Ctrl+Shift+B`.
5. Confirm that the Output window reports a successful build with no errors.

The compiled application is written to `MunicipalCitizenReporting\bin\Debug\MunicipalCitizenReporting.exe` for a Debug build or the corresponding `bin\Release` directory for a Release build.

## How to run

1. Open `MunicipalCitizenReporting.sln` in Visual Studio.
2. Build the solution.
3. Press `F5` to start with debugging, or `Ctrl+F5` to start without debugging.
4. The **Municipal Citizen Services** main menu appears first.

## How to use

1. On the main menu, select **Report Issues**.
2. Enter the location where the issue occurred.
3. Select a municipal category.
4. Enter a clear, detailed description.
5. Optionally choose **Attach File** and select a JPG, JPEG, PNG, PDF, DOC, or DOCX file. The selected filename is displayed. Choose **Remove** to clear it.
6. Follow the completion bar and encouraging message as the required fields are completed. A valid report reaches 100%; the attachment remains optional.
7. Select **Submit Report**. Missing required information is highlighted and explained.
8. After a valid submission, note the generated `REQ-####` reference number and `Submitted` status. The form resets for another report.
9. Select **View Captured Issues** to open the read-only session history. It shows the reference number, location, category, description, date reported, status, and attachment filename for every report submitted while the application is running.
10. Select **Back to Main Menu** (or press `Esc`) to return without closing the application.

## Data storage

Reports are stored in memory in a `List<ReportedIssue>` managed by the shared `IssueRepository`. Reports remain available while the application is running and while the user moves between forms. Because this version intentionally has no database, all report data and reference-number state reset when the application closes.

`List<ReportedIssue>` is appropriate for Part 1 because it keeps multiple strongly typed reports in submission order, supports straightforward additions and read-only display, and does not introduce persistence that is outside the assignment scope. The singleton `IssueRepository.Instance` is the application's one shared source of session reports.

The optional attachment is represented by its selected local file path. The application does not upload or copy the file.

## User engagement strategy

The selected strategy is **Service-Request Tracking and Status Updates**. Every successful submission receives a unique `REQ-####` reference number, a date and time, and the initial status `Submitted`. The confirmation message displays the reference and status, and **View Captured Issues** provides a read-only history of captured requests. These features give residents evidence that their issue was formally recorded without implementing the later full status-management workflow.

## Future functionality

The main menu displays these planned options, but they are intentionally disabled for later project parts:

- Local Events and Announcements
- Service Request Status

The separate `Models`, `Data`, and `Forms` folders keep the application straightforward to extend when those features are introduced.
