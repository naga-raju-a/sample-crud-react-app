import React, { useEffect, useState } from 'react';
import {
  Dialog, DialogTitle, DialogContent, DialogActions,  Button,
} from '@mui/material';
import ReusableTextField from './ReusableTextField';
import { saveCafe } from '../../services/api';
import { validateName, validateDescription } from '../../utils/validation';
const defaultForm = {
  name: '', description: '', location: '', logo: ''
};

function CafeDialog({ open, onClose, formData = {}, onSave }) {
  const [form, setForm] = useState({ ...defaultForm });
  const [errors, setErrors] = useState({}); 

  useEffect(() => {
    setForm({ ...defaultForm, ...formData });   
     const newErrors = {};
    setErrors(newErrors);
  }, [formData]);

  const handleOnChange = (e) => {
    if (!e?.target?.name) return;
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
    if (value?.trim()) {
      setErrors((prev) => ({ ...prev, [name]: '' }));
    }
  };

  const validateForm = () => {
    const newErrors = {};
    if (!form.name?.trim()){ 
      newErrors.name = 'Please enter name';
    }else if (!validateName(form.name)) {newErrors.name = 'Name must be 4-20 characters'};
    if (!form.description?.trim()) newErrors.description = 'Please enter description';
    if (!form.location?.trim()) newErrors.location = 'Please enter location';   
    
    if (!validateDescription(form.description)) newErrors.description = 'Max 256 characters';
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async () => {
    if (!validateForm()) return;
    const inputForm = {
    name: form.name?.trim() || '',
    description: form.description?.trim() || '',
    location: form.location?.trim() || '',
    logo: form.logo?.trim() || '',
    ...(form.id && { id: form.id })
    };
    try {
    
      const res = await saveCafe(inputForm);
      if (res?.data) {
        onSave?.();
        onClose();
      } else {
        alert('Failed to save cafe.');
      }
    } catch (error) {
      console.error(error);
      alert('An error occurred while saving the cafe.');
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>{form.id ? 'Edit' : 'Add'} Cafe</DialogTitle>
      <DialogContent dividers>
      <ReusableTextField label="Name" name="name" value={form.name} onChange={handleOnChange} error={errors.name} required  />
      <ReusableTextField label="Description" name="description" value={form.description} onChange={handleOnChange} error={errors.description} required  />
      <ReusableTextField label="Location" name="location" value={form.location} onChange={handleOnChange} error={errors.location} required  />
      <ReusableTextField label="Logo URL" name="logo" value={form.logo} onChange={handleOnChange} />
      </DialogContent>
      <DialogActions>       
        <Button variant="contained" onClick={handleSubmit}>{form.id ? 'Update' : 'Save'}</Button>
        <Button onClick={onClose}>Cancel</Button>
      </DialogActions>
    </Dialog>
  );
}

export default CafeDialog;
