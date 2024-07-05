PostgreSQL
—
Windows/Cmd
—
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=storedb;Username=postgres;Password=12345678" ^
Npgsql.EntityFrameworkCore.PostgreSQL ^
--output-dir "Models" ^
--context ApplicationDbContext ^
--use-database-names ^
--no-onconfiguring ^
--force

MacOS
—
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=sampledbdotnet;Username=postgres;Password=123456" \
Npgsql.EntityFrameworkCore.PostgreSQL \
--output-dir "Models" \
--context ApplicationDbContext \
--use-database-names \
--no-onconfiguring \
--force