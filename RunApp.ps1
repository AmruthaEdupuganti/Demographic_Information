Write-Host "Starting PolicyGuard Insurance Portal..." -ForegroundColor Cyan

# Start backend
Write-Host "Starting Backend (ASP.NET Core API on http://localhost:5000)..." -ForegroundColor Yellow
$backend = Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\backend\Demo.API'; dotnet run" -PassThru

# Wait for backend to be ready
Write-Host "Waiting for backend to be ready..." -ForegroundColor Yellow
$maxAttempts = 60
$backendReady = $false
for ($i = 0; $i -lt $maxAttempts; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5000/swagger" -UseBasicParsing -TimeoutSec 2 -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            Write-Host "Backend is ready!" -ForegroundColor Green
            $backendReady = $true
            break
        }
    } catch {
        Start-Sleep -Seconds 2
    }
}

if (-not $backendReady) {
    Write-Host "Backend did not start within the expected time. Continuing anyway..." -ForegroundColor Red
}

# Install frontend dependencies if needed
$nodeModules = Join-Path $PSScriptRoot "frontend\node_modules"
if (-not (Test-Path $nodeModules)) {
    Write-Host "Installing frontend dependencies..." -ForegroundColor Yellow
    Push-Location "$PSScriptRoot\frontend"
    npm install
    Pop-Location
}

# Start frontend
Write-Host "Starting Frontend (Angular on http://localhost:4200)..." -ForegroundColor Yellow
$frontend = Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PSScriptRoot\frontend'; ng serve" -PassThru

Write-Host ""
Write-Host "Both servers are starting in separate windows." -ForegroundColor Green
Write-Host "  Backend:  http://localhost:5000 (Swagger: http://localhost:5000/swagger)" -ForegroundColor White
Write-Host "  Frontend: http://localhost:4200" -ForegroundColor White
Write-Host ""

# Wait for the frontend to be ready, then open the browser
Write-Host "Waiting for frontend to be ready..." -ForegroundColor Yellow
$maxAttempts = 30
for ($i = 0; $i -lt $maxAttempts; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:4200" -UseBasicParsing -TimeoutSec 2 -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            Write-Host "Frontend is ready! Opening browser..." -ForegroundColor Green
            Start-Process "http://localhost:4200"
            break
        }
    } catch {
        Start-Sleep -Seconds 2
    }
}

Write-Host "Close the spawned terminal windows to stop the servers." -ForegroundColor Gray
