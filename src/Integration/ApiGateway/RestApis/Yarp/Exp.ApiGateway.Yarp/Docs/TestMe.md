### ✅ Test After This Setup

Run both services:

- Your Hour Tracker API at: `https://localhost:7198`
- Your API Gateway at: `https://localhost:8502`

---

Then hit the following proxy URLs and verify they correctly forward to the backend:

| Proxy URL                                 | Target Backend URL                          |
|-------------------------------------------|--------------------------------------------|
| `https://localhost:8502/api/v1/auths/login`   | `https://localhost:7198/api/v1/auths/login`    |
| `https://localhost:8502/api/v1/customers`      | `https://localhost:7198/api/v1/customers`       |
| `https://localhost:8502/api/v1/customers/1`    | `https://localhost:7198/api/v1/customers/1`     |
