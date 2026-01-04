import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './pages/layout/MainLayout';
import Login from './pages/Login';
import FacilityList from './pages/facilities/FacilityList';
import FacilityDetail from './pages/facilities/FacilityDetail';
import BookingList from './pages/bookings/BookingList';
import CreateBooking from './pages/bookings/CreateBooking';
import Administration from './pages/Administration';
import { authService } from './services/auth';
import { useUser } from './hooks/useUser';

function Home() {
  const { user, loading, isAdmin, isStudent, isTeacher } = useUser();

  if (loading) {
    return (
      <div style={{ padding: 24, textAlign: 'center' }}>
        <p>Loading...</p>
      </div>
    );
  }

  return (
    <div style={{ padding: 24 }}>
      <h1>Welcome to ServiceDesk{user?.userName ? `, ${user.userName}` : ''}</h1>
      <p>Facility Booking Management System</p>
      
      {isAdmin && (
        <div style={{ marginTop: 16, padding: 16, background: '#e6f7ff', border: '1px solid #91d5ff', borderRadius: 4 }}>
          <h3 style={{ marginTop: 0, color: '#1890ff' }}>Administrator Dashboard</h3>
          <p>You have full access to manage facilities, bookings, and system settings.</p>
          <ul>
            <li>Manage all facilities (Create, Edit, Delete)</li>
            <li>View and approve all bookings</li>
            <li>Access administration panel</li>
          </ul>
        </div>
      )}

      {isTeacher && (
        <div style={{ marginTop: 16, padding: 16, background: '#f6ffed', border: '1px solid #b7eb8f', borderRadius: 4 }}>
          <h3 style={{ marginTop: 0, color: '#52c41a' }}>Teacher Account</h3>
          <p>You can browse facilities and create bookings, including laboratory facilities.</p>
        </div>
      )}

      {isStudent && (
        <div style={{ marginTop: 16, padding: 16, background: '#fff7e6', border: '1px solid #ffd591', borderRadius: 4 }}>
          <h3 style={{ marginTop: 0, color: '#fa8c16' }}>Student Account</h3>
          <p>You can browse facilities and create bookings. <strong>Note:</strong> Laboratory facilities are restricted to teachers and administrators only.</p>
        </div>
      )}

      <div style={{ marginTop: 24 }}>
        <h3>Quick Actions</h3>
        <ul>
          <li><a href="/facilities">Browse Facilities</a></li>
          <li><a href="/bookings/create">Create Booking</a></li>
          {isAdmin && <li><a href="/administration">Administration Panel</a></li>}
        </ul>
      </div>
    </div>
  );
}

function ProtectedRoute({ children }: { children: React.ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = React.useState(false);
  const [loading, setLoading] = React.useState(true);

  React.useEffect(() => {
    setIsAuthenticated(authService.isAuthenticated());
    setLoading(false);
  }, []);

  if (loading) {
    return <div style={{ padding: 50, textAlign: 'center' }}>Loading...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}


export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <MainLayout />
            </ProtectedRoute>
          }
        >
          <Route index element={<Home />} />
          <Route path="facilities" element={<FacilityList />} />
          <Route path="facilities/:id" element={<FacilityDetail />} />
          <Route path="bookings" element={<BookingList />} />
          <Route path="bookings/create" element={<CreateBooking />} />
          <Route path="administration" element={<Administration />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
