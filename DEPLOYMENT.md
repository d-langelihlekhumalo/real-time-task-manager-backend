# 🚀 Coolify Deployment Guide - Real-Time Task Manager API

## 📦 Deployment Summary

Your Real-Time Task Manager API is ready for deployment to Coolify using Docker. The Dockerfile has been configured to:

- ✅ Build and publish the ASP.NET Core 8.0 application
- ✅ Expose Swagger UI on production
- ✅ Run on port 8080 (HTTP)
- ✅ Include health checks at `/health`
- ✅ Support SignalR WebSocket connections
- ✅ Auto-create logs directory

---

## 🔧 Required Environment Variables in Coolify

### **1. Database Connection (REQUIRED)**

```
ConnectionStrings__DefaultConnection=Host=your-postgres-host;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=your-password;
```

📝 **Note**: Replace with your actual PostgreSQL connection details from Coolify

- Use the **internal** PostgreSQL URL from Coolify (e.g., `postgresql-xxxxx`)
- Database name: `RealTimeTaskManager`
- Default port: `5432`

### **2. ASP.NET Core Environment**

```
ASPNETCORE_ENVIRONMENT=Production
```

✅ Already set in Dockerfile, but can override if needed

### **3. CORS Origins (REQUIRED for Frontend)**

```
Cors__AllowedOrigins__0=https://your-frontend-domain.com
Cors__AllowedOrigins__1=https://www.your-frontend-domain.com
```

📝 **Note**: Add your actual frontend domain(s)

### **4. Allowed Hosts**

```
AllowedHosts=*
```

✅ Optional - defaults to "\*" (all hosts)

---

## 🌐 Optional Environment Variables

### **5. Swagger UI Control**

```
SwaggerUI__Enabled=true
```

✅ Already enabled - set to `false` if you want to disable Swagger in production

### **6. Health Check Settings**

```
HealthChecks__Enabled=true
HealthChecks__DetailedErrors=false
```

✅ Already configured in appsettings.Production.json

### **7. CORS Credentials**

```
Cors__AllowCredentials=true
```

✅ Already enabled for SignalR WebSocket support

### **8. Logging Level**

```
Logging__LogLevel__Default=Information
Logging__LogLevel__Microsoft__AspNetCore=Warning
```

✅ Already configured - adjust if needed

---

## 📋 Coolify Deployment Steps

1. **Create New Application in Coolify**

   - Go to your Coolify dashboard
   - Click "Add New Resource" → "Application"
   - Select "Docker" as the build pack

2. **Connect GitHub Repository**

   - Repository: `https://github.com/d-langelihlekhumalo/real-time-task-manager-backend.git`
   - Branch: `main`
   - Build Pack: `Dockerfile`

3. **Configure Environment Variables**

   - Go to "Environment Variables" section
   - Add all REQUIRED variables listed above
   - Add optional variables as needed

4. **Set Port Configuration**

   - Application Port: `8080`
   - Public Port: `443` (HTTPS) or `80` (HTTP)
   - Enable WebSocket support (for SignalR)

5. **Configure Domain**

   - Set your custom domain or use Coolify's subdomain
   - Example: `api.yourdomain.com`
   - Enable SSL/TLS certificate

6. **Deploy**
   - Click "Deploy"
   - Monitor build logs
   - Wait for deployment to complete

---

## 🔍 Post-Deployment Verification

### **Test Endpoints:**

1. **Health Check**

   ```
   https://your-domain.com/health
   ```

   Expected: `{"status":"Healthy"}`

2. **Swagger UI**

   ```
   https://your-domain.com/swagger
   ```

   Expected: Interactive API documentation

3. **SignalR Hub**

   ```
   https://your-domain.com/taskManagerHub
   ```

   Expected: WebSocket connection (test with frontend)

4. **API Endpoint**
   ```
   GET https://your-domain.com/api/Dashboard
   ```
   Expected: Dashboard statistics JSON

---

## 🗄️ Database Setup

### **PostgreSQL in Coolify (Recommended)**

1. **Create PostgreSQL Database in Coolify**

   - Go to Databases → Add New → PostgreSQL
   - Select "PostgreSQL 17 (default)"
   - Wait for deployment to complete

2. **Get Connection Details**

   - Copy the internal PostgreSQL URL (e.g., `postgresql-database-xxxxx`)
   - Note the username (usually `postgres`)
   - Copy the generated password
   - Database will be auto-created on first run

3. **Connection String Format:**

```
Host=postgresql-database-xxxxx;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=your-generated-password;
```

### **External PostgreSQL Options:**

1. **Managed PostgreSQL Services**

   - AWS RDS PostgreSQL
   - Azure Database for PostgreSQL
   - Google Cloud SQL PostgreSQL
   - DigitalOcean Managed PostgreSQL

2. **Connection String Example:**

```
Host=your-server.postgres.database.azure.com;Port=5432;Database=RealTimeTaskManager;Username=adminuser@servername;Password=your-password;SslMode=Require;
```

---

## 🔐 Security Checklist

- ✅ Use HTTPS (SSL/TLS) in production
- ✅ Store connection string as environment variable (not in code)
- ✅ Restrict CORS to specific frontend domains
- ✅ Use strong database password
- ✅ Enable SQL Server firewall rules
- ✅ Keep Swagger enabled only if needed (or restrict access)
- ✅ Monitor logs for security issues

---

## 📊 Monitoring & Logs

### **View Application Logs:**

- In Coolify dashboard → Your App → Logs
- Logs are also written to `/app/logs/` inside container

### **Health Monitoring:**

- Health check runs every 30 seconds
- Available at: `https://your-domain.com/health`

### **Performance Monitoring:**

- Monitor database connections
- Check SignalR connection counts
- Review API response times in logs

---

## 🔄 Auto-Deployment

Coolify can auto-deploy on git push:

1. Enable "Auto Deploy" in Coolify settings
2. Push changes to `main` branch
3. Coolify automatically rebuilds and redeploys

---

## 🆘 Troubleshooting

### **Database Connection Failed**

- Verify connection string is correct
- Check SQL Server firewall rules
- Ensure TrustServerCertificate=true for self-signed certs

### **Swagger Not Loading**

- Verify `SwaggerUI__Enabled=true`
- Check application logs for errors
- Ensure port 8080 is accessible

### **SignalR Connection Failed**

- Enable WebSocket support in Coolify
- Verify CORS origins include frontend domain
- Check `AllowCredentials=true` in CORS config

### **Container Health Check Failing**

- Check `/health` endpoint manually
- Verify database is accessible
- Review container logs

---

## 📝 Quick Reference

| Item             | Value           |
| ---------------- | --------------- |
| **Docker Port**  | 8080            |
| **Health Check** | /health         |
| **Swagger UI**   | /swagger        |
| **SignalR Hub**  | /taskManagerHub |
| **API Base**     | /api            |
| **Framework**    | .NET 8.0        |

---

## ✅ Minimum Required Env Vars (Quick Copy)

```bash
# Database (REQUIRED) - Use your PostgreSQL details from Coolify
ConnectionStrings__DefaultConnection=Host=postgresql-database-xxxxx;Port=5432;Database=RealTimeTaskManager;Username=postgres;Password=your-generated-password;

# CORS (REQUIRED for Frontend)
Cors__AllowedOrigins__0=https://your-frontend-domain.com

# Optional
ASPNETCORE_ENVIRONMENT=Production
SwaggerUI__Enabled=true
HealthChecks__Enabled=true
```

---

**🎉 Your backend is ready to deploy!** Just add the environment variables in Coolify and click deploy.
