let date = new Date;
let hour = date.getHours();
let min = date.getMinutes();
let minTime = date.getHours().toString() + ":" + date.getMinutes().toString();

flatpickr.l10ns.default.firstDayOfWeek = 1; // Monday default is sunday

document.getElementById("flatpickrcontainerReminderDate").flatpickr({
    wrap: true,
    minDate: "today",
    weekNumbers: true,
    enableTime: true, // enables timepicker default is false
    time_24hr: true, // set to false for AM PM default is false
    defaultDate: "today",
});

document.getElementById("flatpickrcontainerDueDate").flatpickr({
    wrap: true,
    minDate: "today",
    weekNumbers: true,
    enableTime: true, // enables timepicker default is false
    time_24hr: true, // set to false for AM PM default is false
    defaultDate: "today",
});