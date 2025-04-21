

import React, { useState } from 'react';
import { View, Text, TextInput, TouchableOpacity, Image, Alert, ActivityIndicator, StyleSheet, ScrollView, Dimensions } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import * as FileSystem from 'expo-file-system';
import { supabase } from './supabaseClient';

const AddProduct = ({ navigation }) => {
  const [form, setForm] = useState({
    productName: '',
    description: '',
    price: '',
    stock: '',
    categoryId: '',
  });
  const [image, setImage] = useState(null);
  const [uploading, setUploading] = useState(false);

  // Request permission and pick image
  const pickImage = async () => {
    try {
      console.log('Attempting to request media library permissions...');
      const { status } = await ImagePicker.requestMediaLibraryPermissionsAsync();
      console.log('Permission status:', status);

      if (status !== 'granted') {
        window.confirm('權限拒絕', '需要訪問相冊權限以選擇圖片');
        return;
      }

      console.log('Launching image library...');
      const result = await ImagePicker.launchImageLibraryAsync({
        mediaTypes: 'images',
        quality: 0.8,
        allowsEditing: true,
        aspect: [4, 3],
      });

      console.log('Image picker result:', result);

      if (!result.canceled) {
        setImage(result.assets[0].uri);
        console.log('Image selected:', result.assets[0].uri);
      } else {
        console.log('Image selection canceled');
      }
    } catch (error) {
      console.error('Error in pickImage:', error);
      window.confirm('錯誤', '選擇圖片時發生錯誤: ' + error.message);
    }
  };

  // Handle form submission
  const handleUpload = async () => {
    if (!validateForm()) return;
    if (!image) {
      window.confirm('錯誤', '請選擇產品圖片');
      return;
    }

    setUploading(true);
    try {
      const imageUrl = await uploadImage(); // Updated function will handle upload
      await insertProduct(imageUrl);

      window.confirm('成功', '產品已成功添加', [
        { text: '確定', onPress: () => navigation.goBack() },
      ]);
      resetForm();
    } catch (error) {
      console.error('Upload error:', error);
      window.confirm('錯誤', error.message || '添加產品時出錯');
    } finally {
      setUploading(false);
    }
  };

  // New uploadImage function
  const uploadImage = async () => {
    if (!form.productName || !form.price || !form.stock || !form.categoryId || !image) {
      window.confirm('錯誤', '請填寫所有必填字段並選擇一張圖片');
      return;
    }

    setUploading(true);

    try {
      // Fetch the image as a blob and convert to base64
      const response = await fetch(image);
      const blob = await response.blob();
      const base64 = await new Promise((resolve) => {
        const reader = new FileReader();
        reader.onloadend = () => resolve(reader.result);
        reader.readAsDataURL(blob);
      });
      const base64Data = base64.split(',')[1];

      console.log(JSON.stringify({ image: base64 }));
      console.log(JSON.stringify({ data: base64Data }));

      // Send base64 to Python server
      const pythonResponse = await fetch('http://localhost:5000/upload', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ image: base64Data }),
      });

      if (!pythonResponse.ok) {
        throw new Error('Failed to upload image to Python server');
      }
      console.log("pythonResponse ======== " + pythonResponse);

      const responseData = await pythonResponse.json();
      const publicUrl = responseData.data?.publicUrl; // Adjust based on your API response structure

      if (!publicUrl) throw new Error('Unable to generate public URL');

      return publicUrl;
    } catch (error) {
      window.confirm('錯誤', error.message || '上傳失敗！');
      throw error; // Re-throw the error for further handling
    } finally {
      setUploading(false);
    }
  };

  // Insert product into database
  const insertProduct = async (imageUrl) => {
    const { error } = await supabase.from('products').insert({
      product_name: form.productName,
      description: form.description || null,
      price: parseFloat(form.price),
      stock: parseInt(form.stock),
      category_id: parseInt(form.categoryId),
      image_data: imageUrl,
    });

    if (error) throw new Error(`數據庫錯誤: ${error.message}`);
  };

  // Validate form fields
  const validateForm = () => {
    const requiredFields = ['productName', 'price', 'stock', 'categoryId'];
    if (requiredFields.some((field) => !form[field].trim())) {
      window.confirm('錯誤', '請填寫所有必填字段');
      return false;
    }
    if (isNaN(form.price) || isNaN(form.stock) || isNaN(form.categoryId)) {
      window.confirm('錯誤', '價格、庫存和分類ID必須為數字');
      return false;
    }
    if (parseFloat(form.price) <= 0 || parseInt(form.stock) < 0 || parseInt(form.categoryId) <= 0) {
      window.confirm('錯誤', '價格必須大於0，庫存不能為負數，分類ID必須為正數');
      return false;
    }
    return true;
  };

  // Reset form after submission
  const resetForm = () => {
    setForm({
      productName: '',
      description: '',
      price: '',
      stock: '',
      categoryId: '',
    });
    setImage(null);
  };

  return (

    <ScrollView style={styles.container}>
      <View style={{ marginBottom: 50 }}>
        <View style={styles.header}>
          <Text style={styles.title}>添加新產品</Text>
        </View>

        <View style={styles.formContainer}>
          <TextInput
            placeholder="產品名稱 *"
            value={form.productName}
            onChangeText={(v) => setForm((p) => ({ ...p, productName: v }))}
            style={styles.input}
            placeholderTextColor="#999"
          />
          <TextInput
            placeholder="描述（可選）"
            value={form.description}
            onChangeText={(v) => setForm((p) => ({ ...p, description: v }))}
            style={[styles.input, styles.textArea]}
            multiline
            numberOfLines={3}
            placeholderTextColor="#999"
          />
          <TextInput
            placeholder="價格（HKD） *"
            value={form.price}
            onChangeText={(v) => setForm((p) => ({ ...p, price: v }))}
            keyboardType="numeric"
            style={styles.input}
            placeholderTextColor="#999"
          />
          <TextInput
            placeholder="庫存數量 *"
            value={form.stock}
            onChangeText={(v) => setForm((p) => ({ ...p, stock: v }))}
            keyboardType="numeric"
            style={styles.input}
            placeholderTextColor="#999"
          />
          <TextInput
            placeholder="分類ID *"
            value={form.categoryId}
            onChangeText={(v) => setForm((p) => ({ ...p, categoryId: v }))}
            keyboardType="numeric"
            style={styles.input}
            placeholderTextColor="#999"
          />
          <Text style={{ marginBottom: 8, color: '#555' }}>(分類ID: 1=武俠, 2=烹飪, 3=編程, 4=歷史, 5=自傳)</Text>


          <TouchableOpacity
            style={[styles.button, uploading && styles.buttonDisabled]}
            onPress={pickImage}
            disabled={uploading}
          >
            <Text style={styles.buttonText}>選擇圖片</Text>
          </TouchableOpacity>

          {image && <Image source={{ uri: image }} style={styles.imagePreview} />}

          <TouchableOpacity
            style={[styles.submitButton, uploading && styles.buttonDisabled]}
            onPress={handleUpload}
            disabled={uploading}
          >
            {uploading ? (
              <ActivityIndicator size="small" color="#fff" />
            ) : (
              <Text style={styles.buttonText}>提交產品</Text>
            )}
          </TouchableOpacity>
        </View>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
    maxHeight: Dimensions.get("window").height,

  },
  header: {
    padding: 20,
    backgroundColor: '#2CB696',
    alignItems: 'center',
  },
  title: {
    fontSize: 24,
    fontWeight: '600',
    color: '#fff',
  },
  formContainer: {
    padding: 20,
  },
  input: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 12,
    marginBottom: 16,
    fontSize: 16,
    borderWidth: 1,
    borderColor: '#ddd',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  textArea: {
    height: 80,
    textAlignVertical: 'top',
  },
  button: {
    backgroundColor: '#4CAF50',
    paddingVertical: 12,
    borderRadius: 8,
    alignItems: 'center',
    marginBottom: 16,
  },
  submitButton: {
    backgroundColor: '#3F51B5',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
    marginTop: 10,
  },
  buttonDisabled: {
    backgroundColor: '#999',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '500',
  },
  imagePreview: {
    width: '100%',
    height: 200,
    borderRadius: 8,
    marginBottom: 16,
    resizeMode: 'cover',
  },
});

export default AddProduct;