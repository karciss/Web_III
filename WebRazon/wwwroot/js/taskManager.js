/**
 * Gestor de Tareas - Script para manejar las operaciones CRUD de tareas
 */

// Almacén de tareas en memoria
let tasks = [];
let lastTaskId = 0;

// Variables para los elementos DOM
const taskTable = document.getElementById('tasksTable');
const taskForm = document.getElementById('newTaskForm');
const editTaskForm = document.getElementById('editTaskForm');
const btnSaveTask = document.getElementById('btnSaveTask');
const btnUpdateTask = document.getElementById('btnUpdateTask');
const filterLinks = document.querySelectorAll('.filter-link');

// Contadores
const totalTasksCount = document.getElementById('totalTasksCount');
const inProgressTasksCount = document.getElementById('inProgressTasksCount');
const urgentTasksCount = document.getElementById('urgentTasksCount');
const completedTasksCount = document.getElementById('completedTasksCount');

// Modales
const taskDetailsModal = document.getElementById('taskDetailsModal');
const nuevaTareaModal = document.getElementById('nuevaTareaModal');
const editarTareaModal = document.getElementById('editarTareaModal');

let taskDetailsBootstrap, nuevaTareaBootstrap, editarTareaBootstrap;

// Cargar tareas existentes
document.addEventListener('DOMContentLoaded', () => {
    // Inicializar los modales de Bootstrap
    if (taskDetailsModal) {
        taskDetailsBootstrap = new bootstrap.Modal(taskDetailsModal);
    }
    
    if (nuevaTareaModal) {
        nuevaTareaBootstrap = new bootstrap.Modal(nuevaTareaModal);
    }
    
    if (editarTareaModal) {
        editarTareaBootstrap = new bootstrap.Modal(editarTareaModal);
    }
    
    // Inicializar el array de tareas con los elementos que ya están en la tabla
    const taskRows = document.querySelectorAll('#tasksTable tbody tr');
    taskRows.forEach(row => {
        const taskId = parseInt(row.getAttribute('data-task-id'));
        const title = row.cells[0].textContent;
        const dueDate = row.cells[1].textContent;
        const status = row.getAttribute('data-task-status');
        
        // Crear objeto de tarea
        const task = {
            id: taskId,
            title: title,
            dueDate: dueDate,
            status: status,
            description: `Descripción para la tarea ${title}` // No tenemos descripción en la tabla, usamos un valor por defecto
        };
        
        // Añadir al array
        tasks.push(task);
        
        // Actualizar el último ID
        if (taskId > lastTaskId) {
            lastTaskId = taskId;
        }
    });

    // Configurar manejadores de eventos
    setupEventHandlers();
    
    // Inicializar campo de fecha con la fecha actual
    const taskDueDateInput = document.getElementById('taskDueDate');
    if (taskDueDateInput) {
        const today = new Date();
        const yyyy = today.getFullYear();
        let mm = today.getMonth() + 1;
        let dd = today.getDate();
        
        if (dd < 10) dd = '0' + dd;
        if (mm < 10) mm = '0' + mm;
        
        taskDueDateInput.value = `${yyyy}-${mm}-${dd}`;
    }
    
    console.log('Gestor de tareas inicializado con', tasks.length, 'tareas');
});

/**
 * Configura todos los manejadores de eventos
 */
function setupEventHandlers() {
    // Evento para guardar una nueva tarea
    if (btnSaveTask) {
        btnSaveTask.addEventListener('click', saveTask);
    }
    
    // Evento para actualizar una tarea existente
    if (btnUpdateTask) {
        btnUpdateTask.addEventListener('click', updateTask);
    }
    
    // Evento para filtrar tareas
    filterLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const filter = link.getAttribute('data-filter');
            filterTasks(filter);
        });
    });
    
    // Evento para completar una tarea
    document.querySelectorAll('.btn-complete-task').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const taskId = parseInt(e.currentTarget.getAttribute('data-task-id'));
            completeTask(taskId);
        });
    });
    
    // Evento para editar una tarea
    document.querySelectorAll('.btn-edit-task').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const taskId = parseInt(e.currentTarget.getAttribute('data-task-id'));
            loadTaskForEditing(taskId);
        });
    });
    
    // Evento para eliminar una tarea
    document.querySelectorAll('.btn-delete-task').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const taskId = parseInt(e.currentTarget.getAttribute('data-task-id'));
            deleteTask(taskId);
        });
    });
    
    // Evento para ver detalles de una tarea
    document.querySelectorAll('.btn-view-task').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const taskId = parseInt(e.currentTarget.getAttribute('data-task-id'));
            viewTaskDetails(taskId);
        });
    });
}

/**
 * Guarda una nueva tarea
 */
async function saveTask() {
    // Validar formulario
    const title = document.getElementById('taskTitle').value.trim();
    const description = document.getElementById('taskDescription').value.trim();
    const dueDate = document.getElementById('taskDueDate').value;
    const status = document.getElementById('taskStatus').value;
    
    if (!title) {
        document.getElementById('taskTitle').classList.add('is-invalid');
        return;
    }
    
    if (!dueDate) {
        document.getElementById('taskDueDate').classList.add('is-invalid');
        return;
    }
    
    // Deshabilitar botón mientras se procesa
    btnSaveTask.disabled = true;
    btnSaveTask.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...';
    
    try {
        // Crear nueva tarea
        const newTask = {
            title: title,
            description: description,
            dueDate: dueDate,
            status: status
        };
        
        // Opción 1: Usar la API del controlador si estamos conectados al backend
        if (typeof fetch === 'function') {
            try {
                const response = await fetch('?handler=AddTask', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        // Incluir el token anti-falsificación si es necesario
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                    },
                    body: JSON.stringify(newTask)
                });
                
                if (response.ok) {
                    const data = await response.json();
                    if (data.success) {
                        // Añadir la tarea con el ID asignado por el servidor
                        const task = data.task;
                        tasks.push(task);
                        addTaskToTable(task);
                        updateTaskCounters();
                        showNotification('Tarea agregada exitosamente');
                        nuevaTareaBootstrap.hide();
                    }
                } else {
                    showNotification('Error al guardar la tarea', 'danger');
                }
            } catch (error) {
                console.error('Error al guardar la tarea:', error);
                // Si falla la llamada a la API, usar la opción de JavaScript local
                fallbackSaveTask(newTask);
            }
        } else {
            // Opción 2: Almacenar localmente (modo offline)
            fallbackSaveTask(newTask);
        }
    } finally {
        // Habilitar botón nuevamente
        btnSaveTask.disabled = false;
        btnSaveTask.innerHTML = '<i class="bi bi-plus-lg me-1"></i> Agregar Tarea';
        
        // Limpiar formulario
        taskForm.reset();
        
        // Restablecer la fecha
        const taskDueDateInput = document.getElementById('taskDueDate');
        if (taskDueDateInput) {
            const today = new Date();
            const yyyy = today.getFullYear();
            let mm = today.getMonth() + 1;
            let dd = today.getDate();
            
            if (dd < 10) dd = '0' + dd;
            if (mm < 10) mm = '0' + mm;
            
            taskDueDateInput.value = `${yyyy}-${mm}-${dd}`;
        }
    }
}

/**
 * Carga los datos de una tarea para editarla
 */
async function loadTaskForEditing(taskId) {
    // Obtener datos de la tarea
    let task;
    
    try {
        // Intentar obtener la tarea del servidor
        const response = await fetch(`?handler=TaskById&id=${taskId}`);
        
        if (response.ok) {
            const data = await response.json();
            if (data.success) {
                task = data.task;
            }
        }
    } catch (error) {
        console.warn('Error al obtener la tarea del servidor, usando datos locales');
    }
    
    // Si no pudimos obtener la tarea del servidor, buscarla en el array local
    if (!task) {
        task = tasks.find(t => t.id === taskId);
    }
    
    if (!task) {
        showNotification('No se pudo encontrar la tarea para editar', 'danger');
        return;
    }
    
    // Rellenar el formulario de edición
    document.getElementById('editTaskId').value = task.id;
    document.getElementById('editTaskTitle').value = task.title;
    document.getElementById('editTaskDescription').value = task.description || '';
    
    // Formatear la fecha para el input date (YYYY-MM-DD)
    let dueDate = task.dueDate;
    if (typeof dueDate === 'string' && dueDate.includes('T')) {
        // Si la fecha tiene formato ISO (con hora)
        dueDate = dueDate.split('T')[0];
    } else if (typeof dueDate === 'string') {
        // Si ya es un string con formato de fecha
        const parts = dueDate.split('-');
        if (parts.length === 3) {
            dueDate = `${parts[0]}-${parts[1]}-${parts[2]}`;
        }
    }
    
    document.getElementById('editTaskDueDate').value = dueDate;
    document.getElementById('editTaskStatus').value = task.status;
    
    // Mostrar modal
    editarTareaBootstrap.show();
}

/**
 * Actualiza una tarea existente
 */
async function updateTask() {
    // Validar formulario
    const id = parseInt(document.getElementById('editTaskId').value);
    const title = document.getElementById('editTaskTitle').value.trim();
    const description = document.getElementById('editTaskDescription').value.trim();
    const dueDate = document.getElementById('editTaskDueDate').value;
    const status = document.getElementById('editTaskStatus').value;
    
    if (!title) {
        document.getElementById('editTaskTitle').classList.add('is-invalid');
        return;
    }
    
    if (!dueDate) {
        document.getElementById('editTaskDueDate').classList.add('is-invalid');
        return;
    }
    
    // Deshabilitar botón mientras se procesa
    btnUpdateTask.disabled = true;
    btnUpdateTask.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...';
    
    try {
        // Crear objeto con los datos actualizados
        const updatedTask = {
            id: id,
            title: title,
            description: description,
            dueDate: dueDate,
            status: status
        };
        
        // Opción 1: Usar la API del controlador si estamos conectados al backend
        if (typeof fetch === 'function') {
            try {
                const response = await fetch('?handler=UpdateTask', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        // Incluir el token anti-falsificación si es necesario
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                    },
                    body: JSON.stringify(updatedTask)
                });
                
                if (response.ok) {
                    const data = await response.json();
                    if (data.success) {
                        // Actualizar la tarea en el array local
                        const taskIndex = tasks.findIndex(t => t.id === id);
                        if (taskIndex !== -1) {
                            tasks[taskIndex] = updatedTask;
                        }
                        
                        // Actualizar la fila en la tabla
                        updateTaskInTable(updatedTask);
                        
                        // Actualizar contadores si el estado cambió
                        updateTaskCounters();
                        
                        showNotification('Tarea actualizada exitosamente');
                        editarTareaBootstrap.hide();
                    }
                } else {
                    showNotification('Error al actualizar la tarea', 'danger');
                }
            } catch (error) {
                console.error('Error al actualizar la tarea:', error);
                // Si falla la llamada a la API, usar la opción de JavaScript local
                fallbackUpdateTask(updatedTask);
            }
        } else {
            // Opción 2: Actualizar localmente (modo offline)
            fallbackUpdateTask(updatedTask);
        }
    } finally {
        // Habilitar botón nuevamente
        btnUpdateTask.disabled = false;
        btnUpdateTask.innerHTML = '<i class="bi bi-save me-1"></i> Guardar Cambios';
    }
}

/**
 * Actualiza una tarea localmente (fallback)
 */
function fallbackUpdateTask(updatedTask) {
    // Actualizar la tarea en el array
    const taskIndex = tasks.findIndex(t => t.id === updatedTask.id);
    
    if (taskIndex !== -1) {
        tasks[taskIndex] = updatedTask;
        
        // Actualizar la fila en la tabla
        updateTaskInTable(updatedTask);
        
        // Actualizar contadores
        updateTaskCounters();
        
        // Cerrar modal
        editarTareaBootstrap.hide();
        
        // Mostrar notificación
        showNotification('Tarea actualizada exitosamente (modo local)');
    }
}

/**
 * Actualiza una tarea en la tabla
 */
function updateTaskInTable(task) {
    const row = document.querySelector(`#tasksTable tbody tr[data-task-id="${task.id}"]`);
    
    if (!row) return;
    
    // Actualizar datos en la fila
    row.cells[0].textContent = task.title;
    row.cells[1].textContent = task.dueDate;
    
    // Determinar clase de badge según el estado
    const badgeClass = task.status === 'Urgente' ? 'bg-danger' : 
                      task.status === 'En Progreso' ? 'bg-warning' : 
                      task.status === 'Completada' ? 'bg-success' : 'bg-info';
    
    // Actualizar la celda de estado
    row.cells[2].innerHTML = `<span class="badge ${badgeClass}">${task.status}</span>`;
    
    // Actualizar el atributo de estado
    row.setAttribute('data-task-status', task.status);
    
    // Aplicar una animación para destacar la fila actualizada
    row.classList.add('task-updated');
    setTimeout(() => {
        row.classList.remove('task-updated');
    }, 2000);
}

/**
 * Guarda una tarea localmente (fallback)
 */
function fallbackSaveTask(newTask) {
    // Crear nueva tarea con ID local
    newTask.id = ++lastTaskId;
    
    // Añadir a la lista de tareas
    tasks.push(newTask);
    
    // Añadir a la tabla
    addTaskToTable(newTask);
    
    // Actualizar contadores
    updateTaskCounters();
    
    // Cerrar modal
    nuevaTareaBootstrap.hide();
    
    // Mostrar notificación
    showNotification('Tarea agregada exitosamente (modo local)');
}

/**
 * Añade una tarea a la tabla
 */
function addTaskToTable(task) {
    const tbody = document.querySelector('#tasksTable tbody');
    
    // Crear fila
    const row = document.createElement('tr');
    row.setAttribute('data-task-id', task.id);
    row.setAttribute('data-task-status', task.status);
    
    // Determinar clase de badge según el estado
    const badgeClass = task.status === 'Urgente' ? 'bg-danger' : 
                       task.status === 'En Progreso' ? 'bg-warning' : 
                       task.status === 'Completada' ? 'bg-success' : 'bg-info';
    
    // Agregar celdas
    row.innerHTML = `
        <td>${task.title}</td>
        <td>${task.dueDate}</td>
        <td>
            <span class="badge ${badgeClass}">${task.status}</span>
        </td>
        <td>
            <div class="btn-group">
                <button class="btn btn-sm btn-outline-primary btn-edit-task" title="Editar" data-task-id="${task.id}" data-bs-toggle="modal" data-bs-target="#editarTareaModal">
                    <i class="bi bi-pencil-square"></i>
                </button>
                <button class="btn btn-sm btn-outline-success btn-complete-task" title="Completar" data-task-id="${task.id}">
                    <i class="bi bi-check-lg"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger btn-delete-task" title="Eliminar" data-task-id="${task.id}">
                    <i class="bi bi-trash"></i>
                </button>
                <button class="btn btn-sm btn-outline-info btn-view-task" title="Ver detalles" data-task-id="${task.id}">
                    <i class="bi bi-info-circle"></i>
                </button>
            </div>
        </td>
    `;
    
    // Añadir al DOM
    tbody.insertBefore(row, tbody.firstChild);
    
    // Configurar eventos para los botones de esta fila
    const editBtn = row.querySelector('.btn-edit-task');
    const completeBtn = row.querySelector('.btn-complete-task');
    const deleteBtn = row.querySelector('.btn-delete-task');
    const viewBtn = row.querySelector('.btn-view-task');
    
    editBtn.addEventListener('click', () => loadTaskForEditing(task.id));
    completeBtn.addEventListener('click', () => completeTask(task.id));
    deleteBtn.addEventListener('click', () => deleteTask(task.id));
    viewBtn.addEventListener('click', () => viewTaskDetails(task.id));
}

/**
 * Filtra las tareas según el estado seleccionado
 */
function filterTasks(filter) {
    // Obtener todas las filas
    const rows = document.querySelectorAll('#tasksTable tbody tr');
    
    rows.forEach(row => {
        const status = row.getAttribute('data-task-status');
        
        if (filter === 'todas' || status === filter) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
    
    // Actualizar la etiqueta del botón de filtrado
    const filterButtonText = document.getElementById('filterDropdown');
    filterButtonText.innerHTML = `<i class="bi bi-funnel"></i> ${filter === 'todas' ? 'Filtrar' : filter}`;
}

/**
 * Marca una tarea como completada
 */
async function completeTask(taskId) {
    try {
        // Buscar la tarea
        const taskIndex = tasks.findIndex(t => t.id === taskId);
        
        if (taskIndex === -1) return;
        
        // Buscar la fila en la tabla
        const row = document.querySelector(`#tasksTable tbody tr[data-task-id="${taskId}"]`);
        
        // Añadir clase para animación
        row.classList.add('task-completed');
        
        // Intentar actualizar en el servidor
        try {
            const response = await fetch(`?handler=UpdateTaskStatus&id=${taskId}&status=Completada`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    // Incluir el token anti-falsificación si es necesario
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                }
            });
            
            if (!response.ok) {
                console.warn('No se pudo actualizar la tarea en el servidor, aplicando cambios localmente');
            }
        } catch (error) {
            console.warn('Error al conectar con el servidor, aplicando cambios localmente');
        }
        
        // Actualizar el estado localmente
        tasks[taskIndex].status = 'Completada';
        
        // Actualizar la fila en la tabla
        row.setAttribute('data-task-status', 'Completada');
        
        // Actualizar el badge
        const statusCell = row.cells[2];
        statusCell.innerHTML = '<span class="badge bg-success">Completada</span>';
        
        // Actualizar contadores
        updateTaskCounters();
        
        // Mostrar notificación
        showNotification('Tarea completada');
    } catch (error) {
        console.error('Error al completar la tarea:', error);
        showNotification('Error al completar la tarea', 'danger');
    }
}

/**
 * Elimina una tarea
 */
async function deleteTask(taskId) {
    // Confirmar eliminación
    if (confirm('¿Estás seguro de que deseas eliminar esta tarea?')) {
        try {
            // Buscar la fila en la tabla
            const row = document.querySelector(`#tasksTable tbody tr[data-task-id="${taskId}"]`);
            
            // Añadir clase para animación
            row.classList.add('task-deleting');
            
            // Esperar a que termine la animación
            setTimeout(async () => {
                // Intentar eliminar en el servidor
                try {
                    const response = await fetch(`?handler=DeleteTask&id=${taskId}`, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                            // Incluir el token anti-falsificación si es necesario
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
                        }
                    });
                    
                    if (!response.ok) {
                        console.warn('No se pudo eliminar la tarea en el servidor, aplicando cambios localmente');
                    }
                } catch (error) {
                    console.warn('Error al conectar con el servidor, aplicando cambios localmente');
                }
                
                // Eliminar del array
                tasks = tasks.filter(t => t.id !== taskId);
                
                // Eliminar de la tabla
                row.remove();
                
                // Actualizar contadores
                updateTaskCounters();
                
                // Mostrar notificación
                showNotification('Tarea eliminada');
            }, 500); // Esperar 500ms para que termine la animación
        } catch (error) {
            console.error('Error al eliminar la tarea:', error);
            showNotification('Error al eliminar la tarea', 'danger');
        }
    }
}

/**
 * Muestra los detalles de una tarea
 */
function viewTaskDetails(taskId) {
    // Buscar la tarea
    const task = tasks.find(t => t.id === taskId);
    
    if (task) {
        // Actualizar el modal con los detalles
        document.getElementById('detailsTitle').textContent = task.title;
        document.getElementById('detailsDescription').textContent = task.description || 'Sin descripción';
        document.getElementById('detailsDueDate').textContent = task.dueDate;
        
        // Crear badge para el estado
        const badgeClass = task.status === 'Urgente' ? 'bg-danger' : 
                          task.status === 'En Progreso' ? 'bg-warning' : 
                          task.status === 'Completada' ? 'bg-success' : 'bg-info';
                          
        document.getElementById('detailsStatus').innerHTML = `<span class="badge ${badgeClass}">${task.status}</span>`;
        
        // Mostrar el modal
        taskDetailsBootstrap.show();
    }
}

/**
 * Actualiza los contadores de tareas
 */
function updateTaskCounters() {
    const totalCount = tasks.length;
    const inProgressCount = tasks.filter(t => t.status === 'En Progreso').length;
    const urgentCount = tasks.filter(t => t.status === 'Urgente').length;
    const completedCount = tasks.filter(t => t.status === 'Completada').length;
    
    if (totalTasksCount) totalTasksCount.textContent = totalCount;
    if (inProgressTasksCount) inProgressTasksCount.textContent = inProgressCount;
    if (urgentTasksCount) urgentTasksCount.textContent = urgentCount;
    if (completedTasksCount) completedTasksCount.textContent = completedCount;
}

/**
 * Muestra una notificación
 */
function showNotification(message, type = 'success') {
    // Crear notificación
    const notification = document.createElement('div');
    notification.className = `toast align-items-center text-white bg-${type}`;
    notification.setAttribute('role', 'alert');
    notification.setAttribute('aria-live', 'assertive');
    notification.setAttribute('aria-atomic', 'true');
    
    notification.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;
    
    // Contenedor para las notificaciones (si no existe, crearlo)
    let toastContainer = document.querySelector('.toast-container');
    
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        document.body.appendChild(toastContainer);
    }
    
    // Añadir la notificación al contenedor
    toastContainer.appendChild(notification);
    
    // Mostrar la notificación
    const toast = new bootstrap.Toast(notification, {
        autohide: true,
        delay: 3000
    });
    
    toast.show();
    
    // Eliminar la notificación cuando se oculte
    notification.addEventListener('hidden.bs.toast', () => {
        notification.remove();
    });
}

// Reiniciar validación cuando el usuario interactúa con los campos
document.querySelectorAll('.form-control').forEach(input => {
    input.addEventListener('input', () => {
        input.classList.remove('is-invalid');
    });
});