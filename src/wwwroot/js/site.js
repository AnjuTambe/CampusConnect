// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Global variable to track admin mode state
let isAdminModeOn = false; // Default state

// Function to update visibility of admin controls
function updateAdminControlsVisibility() {
    const adminControls = document.querySelectorAll('.admin-control');
    adminControls.forEach(control => {
        if (isAdminModeOn) {
            control.classList.remove('d-none');
        } else {
            control.classList.add('d-none');
        }
    });
}

// Function to update the toggle button's appearance
function updateAdminToggleButtonAppearance() {
    const adminToggleButtons = [
        document.getElementById('adminModeToggle'),
        document.getElementById('adminModeToggleEmployers')
    ];
    
    adminToggleButtons.forEach(adminToggleButton => {
        if (adminToggleButton) { // Check if the button exists on the current page
            if (isAdminModeOn) {
                adminToggleButton.textContent = 'Admin Mode: ON';
                adminToggleButton.classList.remove('btn-outline-secondary');
                adminToggleButton.classList.add('btn-success');
            } else {
                adminToggleButton.textContent = 'Admin Mode: OFF';
                adminToggleButton.classList.remove('btn-success');
                adminToggleButton.classList.add('btn-outline-secondary');
            }
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    // Load state from sessionStorage
    const storedAdminMode = sessionStorage.getItem('isAdminModeOn');
    if (storedAdminMode !== null) {
        isAdminModeOn = JSON.parse(storedAdminMode); // Parse string back to boolean
    }

    // Handle both admin toggle buttons
    const adminToggleButtons = [
        document.getElementById('adminModeToggle'),
        document.getElementById('adminModeToggleEmployers')
    ];

    adminToggleButtons.forEach(adminToggleButton => {
        if (adminToggleButton) {
            adminToggleButton.addEventListener('click', function () {
                isAdminModeOn = !isAdminModeOn; // Toggle the state
                sessionStorage.setItem('isAdminModeOn', JSON.stringify(isAdminModeOn)); // Save new state
                updateAdminToggleButtonAppearance();
                updateAdminControlsVisibility(); 
            });
        }
    });

    // Initial UI update based on loaded/default state
    updateAdminToggleButtonAppearance(); // Update button if it exists
    updateAdminControlsVisibility(); // Update visibility of all admin controls

    // Scroll Animation Observer
    const observerOptions = {
        root: null,
        rootMargin: '0px',
        threshold: 0.1
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target); // Only animate once
            }
        });
    }, observerOptions);

    const animatedElements = document.querySelectorAll('.animate-on-scroll');
    animatedElements.forEach((el) => {
        observer.observe(el);
    });
});