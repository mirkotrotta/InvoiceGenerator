# InvoiceGenerator
## About The Project
InvoiceGenerator is an ASP.NET Core MVC application designed to help German freelancers generate and manage invoices efficiently. This project leverages C#, .NET 8, and Visual Studio 2022 to provide a robust solution for creating detailed invoices and calculating VAT.

### Key Features
- **Client Management**: Add, edit, and delete client information.
- **Invoice Management**: Create, view, edit, and delete invoices.
- **Line Items**: Add services or products to invoices with quantity and unit price.
- **VAT Calculation**: Automatically calculate VAT based on German regulations.

## Getting Started
### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/mirkotrotta/InvoiceGenerator.git
   ```
2. **Navigate to the project directory**:
   ```bash
   cd InvoiceGenerator
   ```
3. **Set up the database**:
   - Update the connection string in `appsettings.json`.
   - Run the following commands in the Package Manager Console:
     ```powershell
     Update-Database
     ```
4. **Run the application**:
   - Open the solution in Visual Studio 2022.
   - Press `F5` to build and run the project.

## Usage
### Creating Clients
1. Navigate to the **Clients** section.
2. Click on **Add New Client**.
3. Fill in the client's details and click **Save**.

### Creating Invoices
1. Navigate to the **Invoices** section.
2. Click on **Create New Invoice**.
3. Select a client, add line items, and click **Save**.

## Roadmap
- **User Authentication**: Implement user authentication and authorization using ASP.NET Core Identity.
- **Localization**: Support for German date and currency formats.
- **Email Integration**: Add functionality to email invoices directly to clients.
- **PDF Generation**: Implement PDF generation for invoices.
- **Enhanced Reporting**: Provide more detailed reports and analytics.

## Contributing
Contributions are what make the open-source community an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.
1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License
Distributed under the MIT License. See `LICENSE` for more information.

## Contact
Mirko Trotta - [@mirkotrotta](https://twitter.com/mirkotrotta)
mirko@metacubostudio.com
hello@mirkotrotta.com

## Acknowledgments
- [DinkToPdf](https://github.com/rdvojmoc/DinkToPdf)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
- [GitHub Best Practices](https://docs.github.com/en/github)

