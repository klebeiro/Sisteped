import { Provider } from "react-redux";
import "react-toastify/dist/ReactToastify.css";
import "./App.css";
import { AppRouter } from "./components";
import { storeState } from "./store/redux/reduxConfiguration";
import { ToastContainer } from "react-toastify";

function App() {
  return (
    <>
      <Provider store={storeState}>
        <AppRouter />
        <ToastContainer
          position="top-center"
          hideProgressBar={false}
          closeOnClick
          pauseOnHover
          theme="light" 
          icon={false}
          autoClose={3000}
        />
      </Provider>
    </>
  );
}

export default App;
