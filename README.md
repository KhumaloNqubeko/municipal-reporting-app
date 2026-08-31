Municipal Citizen Reporting

Project overview

Municipal Citizen Reporting is a Windows Forms application written in C# which enables residents to report municipal service problems, for example potholes, water leaks, electrical faults, sanitation issues, and streetlight faults. Within the present academic project scope, the function for reporting issues is implemented while the other municipal services that were planned for inclusion are displayed but not available.

The system gives each report a unique reference number beginning with REQ-1001 and states that it has been Submitted when a resident makes a submission. The report includes various important details such as the reference number, the location, the municipal category, a description, any supporting files that have been attached, the date and time of submission, and its present status.

Residents have the option of looking at a read-only record of all the reports that they have submitted during the present application session.

Requirements

* Windows 10 or Windows 11
* A version of Visual Studio 2022 or a more recent version of Visual Studio that supports desktop development for the .NET Framework
* The .NET desktop development Visual Studio workload
* .NET Framework 4.8 Developer Pack or targeting pack

No database, web service, third-party package, or internet connection is required.

How to compile

1. Open MunicipalCitizenReporting.sln in Visual Studio.
2. When Visual Studio asks for missing components, you should use the Visual Studio Installer to install the .NET desktop development workload and the .NET Framework 4.8 targeting pack.
3. Choose 'Debug' and 'Any CPU' from the solution configuration options.
4. Select Build > Build Solution or press Ctrl+Shift+B.
5. Make sure that the Output window indicates that the build has been successful with no errors.

For a Debug build the compiled application is written to MunicipalCitizenReporting\bin\Debug\MunicipalCitizenReporting.exe while for a Release build it is written to the corresponding bin\Release directory.

How to run

1. Open MunicipalCitizenReporting.sln in Visual Studio.
2. Build the solution.
3. To begin debugging press F5, or press Ctrl+F5 if you want to start without debugging.
4. The main menu for Municipal Citizen Services is displayed first.

How to use

1. To select Report Issues, go to the main menu.
2. Input the location of where the problem took place.
3. Select a municipal category.
4. Type a clear and detailed description.
5. You may attach a file by selecting Attach File and then picking a JPG, JPEG, PNG, PDF, DOC, or DOCX file; the name of the selected file is shown. You can choose Remove to clear it.
6. As you fill in the required fields, please follow the completion bar and the encouraging message. The report has to reach 100% while the attachment is still optional.
7. Choose the option Submit Report; if any required information is missing it will be highlighted and explained.
8. Once you have made a valid submission, please note the generated REQ-#### reference number and confirm that the status is 'Submitted'. The form will then reset so that you can prepare another report.
9. To open the read-only session history choose View Captured Issues. This display shows the reference number, location, category, description, date reported, status, and attachment filename for each report that was submitted while the application was running.
10. To go back to the main menu (or press Esc) without quitting the application.

Data storage

The reports are kept in memory within a List<ReportedIssue> which is managed by the shared IssueRepository. The reports continue to be accessible both while the application is running and when the user navigates between different forms. Since this version deliberately does not include a database, all the report data and the reference-number state are reset when the application is closed.

List<ReportedIssue> should be used in Part 1 since it maintains the various strongly typed reports in the order in which they are submitted, allows for simple additions and provides for read-only display, and doesn't include any kind of persistence that is outside the scope of the assignment—the singleton IssueRepository.Instance is the application's single common source of session reports.

The optional attachment is shown by the path to the local file that has been chosen. The programme neither uploads nor copies the file.

User engagement strategy

The strategy chosen is Service-Request Tracking and Status Updates. Each time a submission is successfully made, it is given a unique REQ-#### reference number together with a date and time and the initial status of Submitted. The confirmation message shows the reference and status, while View Captured Issues offers a read-only record of the captured requests. Because of these features, residents have proof that their complaint has been officially recorded even though the full status-management workflow is not carried out.

Future functionality

The main menu displays these planned options, but they are intentionally disabled for later project parts:

* Local Events and Announcements
* Service Request Status

The separate Models, Data, and Forms folders keep the application straightforward to extend when those features are introduced.