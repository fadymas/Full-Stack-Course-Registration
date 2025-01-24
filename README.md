# Student Management System

## Overview

This project is a **Student Management System** built using ASP.NET Core with authentication and authorization. It allows administrators to manage students, courses, departments, and user roles. The system supports CRUD operations for students, courses, and departments, and provides a user-friendly interface for managing student courses and departments.

## Features

- **User Authentication and Authorization**:
  - Users can register, log in, and reset their passwords.
  - Admin users have access to manage students, courses, departments, and other users.
  - Students can view their enrolled courses.

- **Student Management**:
  - Admins can add, edit, and delete students.
  - Students can be assigned to departments and courses.
  - Admins can manage student courses (add/remove courses).

- **Course Management**:
  - Admins can create, edit, and delete courses.
  - Courses are associated with departments.

- **Department Management**:
  - Admins can create, edit, and delete departments.
  - Departments have associated fees and courses.

- **Admin Management**:
  - Admins can add, edit, and delete other admin users.
  - Admins can assign or revoke admin privileges to users.

- **Password Reset**:
  - Users can reset their passwords via email verification.

- **Responsive UI**:
  - The system uses Bootstrap for a responsive and modern user interface.

## Examples

![Home-DarkMode](https://github.com/user-attachments/assets/43debc7a-fad7-4b70-be28-f1c3abcae7d9)
![Home-LightMode](https://github.com/user-attachments/assets/812ae8c0-cc50-4c65-9304-aef4117b19cc)
![SignUp-DarkMode](https://github.com/user-attachments/assets/48cfc877-5427-44c5-b7a8-b570d1fd16da)
![Login-DarkMode](https://github.com/user-attachments/assets/f6d5841b-89e5-4354-adc1-e460a11b7196)
![Admin-Management-System-DarkMode](https://github.com/user-attachments/assets/588d99fb-40ba-41cf-956b-6d2a165921f9)
![Admin-Management-System-LightMode](https://github.com/user-attachments/assets/1d96213a-1467-4f99-96d1-53f15bfd251e)
![Student-Doesn't-have-courses ](https://github.com/user-attachments/assets/9431477a-77ef-4c19-b1b4-b40b5ff77179)
![Student-have-courses ](https://github.com/user-attachments/assets/1eb84621-aa58-4aca-a37f-1463778dafca)

### Admin Features

- **Add/Edit/Delete Students**: Navigate to the "Students" section to manage student records.
- **Add/Edit/Delete Courses**: Navigate to the "Courses" section to manage courses.
- **Add/Edit/Delete Departments**: Navigate to the "Departments" section to manage departments.
- **Manage Admin Users**: Navigate to the "Admin Users" section to manage admin accounts.

### Student Features

- **View Enrolled Courses**: Students can log in and view their enrolled courses.
- **Password Reset**: Students can reset their passwords via the "Forgot Password" feature.

## Dependencies

- **ASP.NET Core**: The framework used to build the application.
- **Entity Framework Core**: Used for database management and ORM.
- **Bootstrap**: Used for styling and responsive design.
- **jQuery**: Used for client-side scripting.
- **Identity**: Used for user authentication and authorization.

## Future Improvements

- **Email Notifications**: Implement email notifications for password resets and course updates.
- **Role-Based Access Control**: Expand role-based access control to include more granular permissions.
- **Reporting**: Add reporting features for administrators to generate student and course reports.
- **UI Enhancements**: Improve the user interface with more interactive elements and better visual design.


### Time For Develop
- 2 Weeks By Myself
