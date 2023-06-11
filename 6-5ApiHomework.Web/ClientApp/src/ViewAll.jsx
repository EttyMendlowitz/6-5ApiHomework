import React, { useEffect, useState } from 'react';
import axios from 'axios';

const ViewAll = () => {
    const [jokes, setJokes] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const getJokes = async () => {
            const { data } = await axios.get('/api/jokes/viewall');
            setJokes(data);
        }
        getJokes();
        setIsLoading(false);
    }, []);

    return (isLoading? <h1>Loading...</h1> : 
        <div className="container" style={{ marginTop: 60 }}>
        <div className="row">
            <div className="col-md-6 offset-md-3">
                {jokes.map(j => <div key={j.id} className="card card-body bg-light mb-3">
                        <h5>{j.setup}</h5>
                        <h5>{j.punchline}</h5>
                        <span>Likes: {j.userLikedJokes.filter(j => j.liked).length}</span>
                        <br />
                        <span>Dislikes: {j.userLikedJokes.filter(j => !j.liked).length}</span>
                    </div>
                )}
            </div>
        </div>
    </div>
)
}

export default ViewAll;