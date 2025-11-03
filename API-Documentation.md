# Real-Time Task Manager API Documentation

## Base URLs

- **Development HTTP**: `http://localhost:5090`
- **Development HTTPS**: `https://localhost:7075`
- **IIS Express**: `http://localhost:56500`

## SignalR Hub Connection

- **SignalR Hub URL**: `/taskManagerHub`
- **Full Development URL**: `https://localhost:7075/taskManagerHub`

---

## API Endpoints

### 🏠 Dashboard Endpoints

#### Get Dashboard Data

- **URL**: `GET /api/Dashboard`
- **Description**: Get dashboard statistics and overview data
- **Response**: `DashboardResponse`

```json
{
  "totalTasks": 15,
  "completedTasks": 8,
  "pendingTasks": 7,
  "totalNotes": 23,
  "recentActivities": [...],
  "completionRate": 53.33,
  "notesPerTask": 1
}
```

#### Get Recent Activities

- **URL**: `GET /api/Dashboard/{count}`
- **Description**: Get recent activities with specified count (max 100, default 20)
- **Parameters**:
  - `count` (int, path): Number of activities to retrieve
- **Response**: `List<ActivityResponse>`

```json
[
	{
		"id": "uuid",
		"action": 0,
		"entityType": 1,
		"entityId": "uuid",
		"entityTitle": "Task Title",
		"description": "Task created",
		"createdAt": "2025-10-27T10:30:00Z",
		"actionDisplayName": "Task Created",
		"entityTypeDisplayName": "Task"
	}
]
```

---

### 📋 Task Endpoints

#### Get All Tasks

- **URL**: `GET /api/Task`
- **Description**: Retrieve all tasks with their notes
- **Response**: `List<TaskResponse>`

```json
[
  {
    "id": "uuid",
    "title": "Task Title",
    "description": "Task Description",
    "isCompleted": false,
    "createdAt": "2025-10-27T10:30:00Z",
    "updatedAt": "2025-10-27T11:00:00Z",
    "notes": [...]
  }
]
```

#### Get Task by ID

- **URL**: `GET /api/Task/{id}`
- **Description**: Retrieve a specific task by ID
- **Parameters**:
  - `id` (Guid, path): Task ID
- **Response**: `TaskResponse`
- **Status Codes**: 200 (OK), 404 (Not Found)

#### Create Task

- **URL**: `POST /api/Task`
- **Description**: Create a new task
- **Request Body**: `CreateTaskRequest`
- **Response**: `TaskResponse`
- **Status Codes**: 201 (Created), 400 (Bad Request)

```json
// Request Body
{
	"title": "New Task Title",
	"description": "Task description (optional)"
}
```

#### Update Task

- **URL**: `PUT /api/Task/{id}`
- **Description**: Update an existing task
- **Parameters**:
  - `id` (Guid, path): Task ID
- **Request Body**: `UpdateTaskRequest`
- **Response**: `TaskResponse`
- **Status Codes**: 200 (OK), 400 (Bad Request), 404 (Not Found)

```json
// Request Body
{
	"title": "Updated Task Title",
	"description": "Updated description",
	"isCompleted": true
}
```

#### Delete Task

- **URL**: `DELETE /api/Task/{id}`
- **Description**: Delete a task
- **Parameters**:
  - `id` (Guid, path): Task ID
- **Response**: `boolean`
- **Status Codes**: 200 (OK), 400 (Bad Request), 404 (Not Found)

#### Toggle Task Completion

- **URL**: `PATCH /api/Task/{id}/toggle-completion`
- **Description**: Toggle the completion status of a task
- **Parameters**:
  - `id` (Guid, path): Task ID
- **Response**: `boolean`
- **Status Codes**: 200 (OK), 400 (Bad Request), 404 (Not Found)

---

### 📝 Note Endpoints

#### Get Notes by Task ID

- **URL**: `GET /api/Note/task/{id}`
- **Description**: Get all notes for a specific task
- **Parameters**:
  - `id` (Guid, path): Task ID
- **Response**: `List<NoteResponse>`

```json
[
	{
		"id": "uuid",
		"taskId": "uuid",
		"content": "Note content",
		"createdAt": "2025-10-27T10:30:00Z"
	}
]
```

#### Get Note by ID

- **URL**: `GET /api/Note/{id}`
- **Description**: Get a specific note by ID
- **Parameters**:
  - `id` (Guid, path): Note ID
- **Response**: `NoteResponse`
- **Status Codes**: 200 (OK), 404 (Not Found)

#### Create Note

- **URL**: `POST /api/Note`
- **Description**: Create a new note for a task
- **Request Body**: `CreateNoteRequest`
- **Response**: `NoteResponse`
- **Status Codes**: 201 (Created), 400 (Bad Request)

```json
// Request Body
{
	"taskId": "uuid",
	"content": "Note content"
}
```

#### Update Note

- **URL**: `PUT /api/Note/{id}`
- **Description**: Update an existing note
- **Parameters**:
  - `id` (Guid, path): Note ID
- **Request Body**: `UpdateNoteRequest`
- **Response**: `NoteResponse`
- **Status Codes**: 200 (OK), 400 (Bad Request), 404 (Not Found)

```json
// Request Body
{
	"content": "Updated note content"
}
```

#### Delete Note

- **URL**: `DELETE /api/Note/{id}`
- **Description**: Delete a note
- **Parameters**:
  - `id` (Guid, path): Note ID
- **Response**: `boolean`
- **Status Codes**: 200 (OK), 400 (Bad Request), 404 (Not Found)

---

## 📡 SignalR Real-Time Events

### Connection Setup (JavaScript)

```javascript
const connection = new signalR.HubConnectionBuilder()
	.withUrl('https://localhost:7075/taskManagerHub')
	.build()

await connection.start()
```

### Event Listeners

#### Task Events

```javascript
// Task created
connection.on('TaskCreated', (message) => {
	// message: { id, title, description, createdAt }
})

// Task updated
connection.on('TaskUpdated', (message) => {
	// message: { id, title, description, isCompleted, updatedAt }
})

// Task deleted
connection.on('TaskDeleted', (message) => {
	// message: { taskId }
})

// Task completion changed
connection.on('TaskCompletionChanged', (message) => {
	// message: { id, title, isCompleted, updatedAt }
})
```

#### Note Events

```javascript
// Note added
connection.on('NoteAdded', (message) => {
	// message: { id, taskId, content, createdAt }
})

// Note updated
connection.on('NoteUpdated', (message) => {
	// message: { id, taskId, content, updatedAt }
})

// Note deleted
connection.on('NoteDeleted', (message) => {
	// message: { id, taskId }
})
```

#### Activity Events

```javascript
// Activity updates for real-time feed
connection.on('ActivityUpdate', (activity) => {
	// activity: ActivityResponse object
})
```

---

## 📊 Data Models

### TaskResponse

```typescript
interface TaskResponse {
	id: string
	title: string
	description?: string
	isCompleted: boolean
	createdAt: string // ISO date string
	updatedAt: string // ISO date string
	notes: NoteResponse[]
}
```

### NoteResponse

```typescript
interface NoteResponse {
	id: string
	taskId: string
	content: string
	createdAt: string // ISO date string
}
```

### CreateTaskRequest

```typescript
interface CreateTaskRequest {
	title: string
	description?: string
}
```

### UpdateTaskRequest

```typescript
interface UpdateTaskRequest {
	title: string
	description?: string
	isCompleted: boolean
}
```

### CreateNoteRequest

```typescript
interface CreateNoteRequest {
	taskId: string
	content: string
}
```

### UpdateNoteRequest

```typescript
interface UpdateNoteRequest {
	content: string
}
```

### DashboardResponse

```typescript
interface DashboardResponse {
	totalTasks: number
	completedTasks: number
	pendingTasks: number
	totalNotes: number
	recentActivities: ActivityResponse[]
	completionRate: number // percentage
	notesPerTask: number
}
```

### ActivityResponse

```typescript
interface ActivityResponse {
	id: string
	action: number // ActivityActionEnum value
	entityType: number // EntityTypeEnum value
	entityId: string
	entityTitle: string
	description: string
	createdAt: string // ISO date string
	actionDisplayName: string
	entityTypeDisplayName: string
}
```

---

## 🛠️ Frontend Implementation Examples

### Fetch All Tasks

```javascript
async function getAllTasks() {
	const response = await fetch('https://localhost:7075/api/Task')
	const tasks = await response.json()
	return tasks
}
```

### Create a New Task

```javascript
async function createTask(title, description) {
	const response = await fetch('https://localhost:7075/api/Task', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify({
			title: title,
			description: description,
		}),
	})
	const newTask = await response.json()
	return newTask
}
```

### Update Task Completion

```javascript
async function toggleTaskCompletion(taskId) {
	const response = await fetch(
		`https://localhost:7075/api/Task/${taskId}/toggle-completion`,
		{
			method: 'PATCH',
		}
	)
	const result = await response.json()
	return result
}
```

### Add Note to Task

```javascript
async function addNoteToTask(taskId, content) {
	const response = await fetch('https://localhost:7075/api/Note', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify({
			taskId: taskId,
			content: content,
		}),
	})
	const newNote = await response.json()
	return newNote
}
```

---

## 🔧 Error Handling

### Common HTTP Status Codes

- **200 OK**: Request successful
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

### Error Response Format

```json
{
	"message": "Error description",
	"details": "Additional error details"
}
```

---

## 🚀 Getting Started Checklist

1. **Set up base URL**: Use `https://localhost:7075` for development
2. **Install SignalR client**: `npm install @microsoft/signalr`
3. **Connect to SignalR hub**: Use `/taskManagerHub` endpoint
4. **Implement API calls**: Use fetch() or axios for HTTP requests
5. **Handle real-time events**: Set up SignalR event listeners
6. **Error handling**: Implement proper error handling for all API calls
7. **Loading states**: Show loading indicators during API calls
8. **Real-time UI updates**: Update UI when receiving SignalR events

---

## 📚 Additional Notes

- All dates are in UTC format
- GUIDs are used for all entity IDs
- SignalR events are sent to all connected clients
- The API uses JSON for request/response bodies
- CORS is configured for development
- Swagger documentation is available at: `https://localhost:7075/swagger`
