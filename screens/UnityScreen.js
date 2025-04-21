

import React, { useEffect, useState } from "react";
import { View, Text, Button, ActivityIndicator, Dimensions, Modal, TouchableOpacity } from "react-native";
import { Unity, useUnityContext } from "react-unity-webgl";
import { supabase } from './supabaseClient'; // Adjust path to your Supabase client
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function UnityScreen({ navigation }) {
    const [modalVisible, setModalVisible] = useState(false);
    const [products, setProducts] = useState([]);
    const [selectedCategory, setselectedCategory] = useState();
    const [BookId, setBookId] = useState();
    const [BookName1, setBookName1] = useState();

    const [isLoading, setIsLoading] = useState(true);
    const { unityProvider, sendMessage, loadingProgression } = useUnityContext({
        loaderUrl: '/Build/Build/Build.loader.js',
        dataUrl: '/Build/Build/Build.data',
        frameworkUrl: '/Build/Build/Build.framework.js',
        codeUrl: '/Build/Build/Build.wasm',
    });

    const [productNames, setProductNames] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const email = await AsyncStorage.getItem('isLoggedIn');
                if (!email) {
                    navigation.replace('Login');
                    return;
                }

                const { data: userData, error: userError } = await supabase
                    .from('users')
                    .select('user_id')
                    .eq('email', email)
                    .single();

                if (userError || !userData) throw new Error('User not found');
                console.log('🍇 User Data:', userData);

                const userId = userData.user_id;

                const { data: preferences, error: prefError } = await supabase
                    .from('user_preferences')
                    .select('category_id')
                    .eq('user_id', userId);

                if (prefError) throw prefError;
                console.log('🍇 User Preferences:', preferences);

                const categoryIds = preferences.map(p => p.category_id);

                const { data: products, error: productError } = await supabase
                    .from('products')
                    .select('*')
                    .in('category_id', categoryIds);

                if (productError) throw productError;
                console.log('🍇 Products:', products);

                const { data: viewHistory, error: viewError } = await supabase
                    .from('view_history')
                    .select('product_id')
                    .eq('user_id', userId);

                if (viewError) throw viewError;
                console.log('🍇 View History:', viewHistory);

                const viewedProductIds = viewHistory.map(v => v.product_id);

                const filteredProducts = products.filter(product =>
                    !viewedProductIds.includes(product.product_id)
                );

                if (filteredProducts.length > 0) {
                    filteredProducts.forEach(product => {
                        console.log('🍇 Filtered Product Name:', product.product_name);
                    });
                } else {
                    console.log('🍇 No products available after filtering.');
                }

                // Set product names after filtering
                setProductNames(filteredProducts);
                return filteredProducts; // Return filtered products for later use
            } catch (error) {
                console.error('🍇 Error fetching products:', error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, [navigation]);

    useEffect(() => {
        console.log('UpdatedproductNames:', productNames);
    }, [productNames]);

    useEffect(() => {
        const productName = productNames;
        let a, b, c, d, e;
        productName.forEach(product => {
            console.log('🍇 Filtered Product Name:', product.product_name);
            a = product.product_name;
            b = product.product_id;
            c = product.category_id;
            d = product.image_data;
        });

        const checkLoadingProgression = setInterval(() => {
            if (loadingProgression === 1) {
                clearInterval(checkLoadingProgression);
                setTimeout(() => {
                    // Use the latest filtered products directly
                    if (productNames.length > 0) {
                        handleClickSpawnEnemies(a, b, c, d); // Pass product names
                    } else {
                        handleClickSpawnEnemies(); // Pass "b" if no products
                    }
                    setLoading(false);
                }, 2000); // Wait for 2 seconds before calling the function
            }
        }, 200); // Check every 0.2 seconds

        return () => clearInterval(checkLoadingProgression); // Cleanup on component unmount
    }, [loadingProgression, productNames]);



    // useEffect(() => {
    //     const fetchProducts = async () => {
    //       try {
    //         // Get the logged-in user's email
    //         const email = await AsyncStorage.getItem('isLoggedIn');
    //         if (!email) {
    //           navigation.replace('Login');
    //           return;
    //         }

    //         // Fetch user data to get user_id
    //         const { data: userData, error: userError } = await supabase
    //           .from('users')
    //           .select('user_id')
    //           .eq('email', email)
    //           .single();

    //         if (userError || !userData) throw new Error('User not found');
    //         console.log('🍇 User Data:', userData); // Log user data

    //         const userId = userData.user_id;

    //         // Fetch user preferences based on user_id
    //         const { data: preferences, error: prefError } = await supabase
    //           .from('user_preferences') // Corrected table name
    //           .select('category_id')
    //           .eq('user_id', userId);

    //         if (prefError) throw prefError;
    //         console.log('🍇 User Preferences:', preferences); // Log user preferences

    //         // Extract category IDs from user preferences
    //         const categoryIds = preferences.map(p => p.category_id);

    //         // Fetch products based on category_ids
    //         const { data: products, error: productError } = await supabase
    //           .from('products')
    //           .select('*') // Select all fields
    //           .in('category_id', categoryIds);

    //         if (productError) throw productError;
    //         console.log('🍇 Products:', products); // Log products

    //         // Fetch view history for the current user
    //         const { data: viewHistory, error: viewError } = await supabase
    //           .from('view_history')
    //           .select('product_id')
    //           .eq('user_id', userId);

    //         if (viewError) throw viewError;
    //         console.log('🍇 View History:', viewHistory); // Log view history

    //         // Extract viewed product IDs
    //         const viewedProductIds = viewHistory.map(v => v.product_id);

    //         // Filter products to exclude those already viewed
    //         const filteredProducts = products.filter(product => 
    //           !viewedProductIds.includes(product.product_id)
    //         );

    //         // Log the filtered product names
    //         if (filteredProducts.length > 0) {
    //           filteredProducts.forEach(product => {
    //             console.log('🍇 Filtered Product Name:', product.product_name);

    //           });
    //         } else {
    //           console.log('🍇 No products available after filtering.');
    //         }

    //         // Set product names to state
    //         setProductNames(filteredProducts);
    //       } catch (error) {
    //         console.error('🍇 Error fetching products:', error.message);
    //       } finally {
    //         setLoading(false);
    //       }
    //     };

    //     fetchProducts();
    //   }, [navigation]);

    // useEffect(() => {
    //     const checkLoadingProgression = setInterval(() => {
    //         if (loadingProgression === 1) {
    //             clearInterval(checkLoadingProgression);
    //             setTimeout(() => {
    //                 //handleClickSpawnEnemies();
    //                 setIsLoading(false);
    //             }, 2000); // Wait for 1 second before sending the message
    //         }
    //     }, 200); // Check every 0.2 seconds

    //     return () => clearInterval(checkLoadingProgression); // Cleanup on component unmount
    // }, [loadingProgression]);

    // useEffect(() => {
    //     const fetchProducts = async () => {
    //         try {
    //             // Fetch data from the products table
    //             const { data, error } = await supabase.from('products').select('*');

    //             if (error) throw error;

    //             // Set products to state
    //             setProducts(data);

    //             // Log product_id and product_name
    //             data.forEach(product => {
    //                 // console.log('🍉🍉🍉Product ID:', product.product_id);
    //                 // console.log('🍉🍉🍉Product Name:', product.product_name);
    //             });
    //         } catch (error) {
    //             console.error('Error fetching products:', error.message);
    //         }
    //     };

    //     fetchProducts();
    // }, []);

    function handleClickSpawnEnemies(bookName, bookId, bookCategory, imageDataURL) {
        const ImageURL = imageDataURL;

        // const ImageURL = "https://ebvecgyezvakcxlegspv.supabase.co/storage/v1/object/public/image//son_of_heaven_story.jpeg";

        console.log("product     " + bookId);




        setTimeout(() => {


            console.log(imageDataURL);
            if (imageDataURL) {
                setBookId(bookId);
                setBookName1(bookName);

                console.log("Unity is running😀");
                sendMessage("Aika_Sailor_Uniform/Body", "RNsetImage", ImageURL);
                const a = sendMessage("Aika_Sailor_Uniform/Body", "SetTextToSpeak", "呢本新書" + bookName + ",你有冇興趣?");
                console.log("a =: " + a);
                //sendMessage("Aika_Sailor_Uniform/Body", "SetLocalhost", "192.168.43.210");
                const b = sendMessage("Aika_Sailor_Uniform/Body", "StartPerformPickUp");
                console.log("b =: " + b);
                setTimeout(() => {
                    setModalVisible(true);
                }, 5000);
            }

        }, 2000);

    }
    const handleYes = async () => {
        const { data: productData, error: productError } = await supabase
            .from('products')
            .select('*')
            .eq('product_id', BookId);

        if (productError) {
            console.error('Error fetching product data:', productError);
            return; // Handle error appropriately
        }

        console.log('Product Data:', productData + "  11");
        setModalVisible(false); // Close the modal
        navigation.navigate('Product Detail', { product: productData[0] });
    };

    const handleNo = () => {
        setModalVisible(false); // Close the modal
    };
    return (
        <View style={{ flex: 1 }}>
            {isLoading && (
                <ActivityIndicator
                    size="large"
                    color="#0000ff"
                    style={{ position: 'absolute', top: '50%', left: '50%', transform: [{ translateX: -25 }, { translateY: -25 }] }}
                />
            )}
            <Unity
                unityProvider={unityProvider}
                style={{
                    width: '100%',
                    height: '100%',
                    position: 'relative'
                }}
            />
            <Button
                title={"setColor"}
                onPress={handleClickSpawnEnemies}
            />
            {/* Overlay Text */}
            <View style={{
                flex: 1,
                position: 'absolute',

                zIndex: 100,
            }}>
               
                            <Modal
                                animationType="fade"
                                transparent={true}
                                visible={modalVisible} // Control visibility with state
                                onRequestClose={() => setModalVisible(false)} // Close modal on request
                            >
                                <View style={{ position: "absolute", width: "20%", height: "8%", backgroundColor: "white" }}></View>
                                <View style={{
                                    flex: 1,

                                    alignItems: 'center',
                                    backgroundColor: 'rgba(0, 0, 0, 0.2)' // Semi-transparent background
                                }}>
                                    <View style={{
                                        width: 300,
                                        padding: 20,
                                        backgroundColor: "white",
                                        borderRadius: 10,
                                        alignItems: 'center'
                                    }}>
                                        <Text style={{ fontSize: 18, marginBottom: 20 }}>呢本新書{BookName1}，你有冇興趣?</Text>
                                        <View style={{ flexDirection: 'row', justifyContent: 'space-around', width: '100%' }}>
                                            <TouchableOpacity onPress={handleYes} style={{ padding: 10, backgroundColor: '#4CAF50', borderRadius: 5 }}>
                                                <Text style={{ color: 'white' }}>有</Text>
                                            </TouchableOpacity>
                                            <TouchableOpacity onPress={handleNo} style={{ padding: 10, backgroundColor: '#F44336', borderRadius: 5 }}>
                                                <Text style={{ color: 'white' }}>沒有</Text>
                                            </TouchableOpacity>
                                        </View>
                                    </View>
                             
                                </View>
                            </Modal>
                       

            </View>
        </View>
    );
}